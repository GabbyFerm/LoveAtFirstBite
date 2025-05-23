using Application.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Votes.Dtos;

namespace Application.Votes.Commands.CreeateVote
{
    public class CastVoteCommandHandler : IRequestHandler<CastVoteCommand, OperationResult<VoteDto>>
    {
        private readonly IGenericRepository<Vote> _voteRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;

        public CastVoteCommandHandler(
            IGenericRepository<Vote> voteRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Restaurant> restaurantRepository,
            IMapper mapper)
        {
            _voteRepository = voteRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<VoteDto>> Handle(CastVoteCommand request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // Validate User
            var userResult = await _userRepository.GetByIdAsync(request.UserId);
            if (!userResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("User not found.");

            // Validate Restaurant
            var restaurantResult = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (!restaurantResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("Restaurant not found.");

            // Check if user has already voted today
            var existingVote = await _voteRepository
                .AsQueryable()
                .FirstOrDefaultAsync(v => v.UserId == request.UserId && v.VoteDate == today, cancellationToken);

            if (existingVote != null)
            {
                if (existingVote.RestaurantId == request.RestaurantId)
                {
                    return OperationResult<VoteDto>.Failure("You’ve already voted for this restaurant today.");
                }

                // Change vote to a different restaurant
                existingVote.RestaurantId = request.RestaurantId;
                await _voteRepository.UpdateAsync(existingVote, cancellationToken);

                var updatedVoteDto = _mapper.Map<VoteDto>(existingVote);
                return OperationResult<VoteDto>.Success(updatedVoteDto);
            }

            // Create a new vote
            var newVote = new Vote
            {
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                VoteDate = today
            };

            var createdVote = await _voteRepository.AddAsync(newVote, cancellationToken);
            var voteDto = _mapper.Map<VoteDto>(createdVote.Data);

            return OperationResult<VoteDto>.Success(voteDto);
        }
    }
}
