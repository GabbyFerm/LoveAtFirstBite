using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Common;
using Domain.Models;
using Application.Interfaces;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Votes.Commands.CreeateVote
{
    public class CastVoteCommandHandler
        : IRequestHandler<CastVoteCommand, OperationResult<VoteDto>>
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

        public async Task<OperationResult<VoteDto>> Handle(
            CastVoteCommand request,
            CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // 1) Validate User
            var userResult = await _userRepository.GetByIdAsync(request.UserId);
            if (!userResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("User not found.");

            // 2) Validate Restaurant
            var restaurantResult = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (!restaurantResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("Restaurant not found.");

            // 3) Check existing vote this date
            var existingVote = await _voteRepository
                .AsQueryable()
                .FirstOrDefaultAsync(
                    v => v.UserId == request.UserId
                      && v.VoteDate.Date == today,
                    cancellationToken);

            if (existingVote != null)
            {
                if (existingVote.RestaurantId == request.RestaurantId)
                {
                    return OperationResult<VoteDto>.Failure(
                        "You’ve already voted for this restaurant today.");
                }

                // Change vote target; leave Round at its existing value (1)
                existingVote.RestaurantId = request.RestaurantId;
                var updateResult = await _voteRepository.UpdateAsync(existingVote, cancellationToken);

                if (!updateResult.IsSuccess)
                    return OperationResult<VoteDto>.Failure("Failed to update vote.");

                var updatedDto = _mapper.Map<VoteDto>(existingVote);
                return OperationResult<VoteDto>.Success(updatedDto);
            }

            // 4) Create a brand‐new vote
            var newVote = new Vote
            {
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                VoteDate = today,
                // Round is omitted here—CLR default = 1, and DB default enforces 1
            };

            // Persist
            var createResult = await _voteRepository.AddAsync(newVote, cancellationToken);
            if (!createResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("Failed to create vote.");

            // Grab the saved entity
            var savedVote = createResult.Data!;

            // 5) Map to DTO (includes Round)
            var voteDto = _mapper.Map<VoteDto>(savedVote);
            return OperationResult<VoteDto>.Success(voteDto);
        }
    }
}
