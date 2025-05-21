using Application.Interfaces;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Commen.Dtos;

namespace Application.Votes.Commands.CreeateVote
{
    public class CostVoteCommandHandler : IRequestHandler<CastVoteCommand, OperationResult<VoteDto>>
    {
        private readonly IGenericRepository<Vote> _voteRepository;
        IGenericRepository<User> _userRepository;
        IGenericRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;
        public CostVoteCommandHandler(IGenericRepository<Vote> voteRepository, IGenericRepository<User> userRepository, IGenericRepository<Restaurant> restaurantRepository, IMapper mapper)
        {
            _voteRepository = voteRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }


        public async Task<OperationResult<VoteDto>> Handle(CastVoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var today = DateTime.UtcNow.Date;

                // Check if user already voted today
                var existingVote = await _voteRepository
                    .AsQueryable()
                    .FirstOrDefaultAsync(v => v.UserId == request.GetHashCode() && v.VoteDate == today, cancellationToken);

                if (existingVote != null)
                {
                    return OperationResult<VoteDto>.Failure("User has already voted today.");
                }

                // Validate User
                var userResult = await _userRepository.GetByIdAsync(request.GetHashCode());
                if (!userResult.IsSuccess)
                    return OperationResult<VoteDto>.Failure("User not found.");

                // Validate Restaurant
                var restaurantResult = await _restaurantRepository.GetByIdAsync(request.GetHashCode());
                if (!restaurantResult.IsSuccess)
                    return OperationResult<VoteDto>.Failure("Restaurant not found.");

                // Create Vote
                var vote = new Vote
                {
                    UserId = request.GetHashCode(),
                    RestaurantId = request.GetHashCode(),
                    VoteDate = today
                };

                var result = await _voteRepository.AddAsync(vote);
             
                var voteDto = _mapper.Map<VoteDto>(result.Data);
                return OperationResult<VoteDto>.Success(voteDto);
            }
            catch (Exception ex)
            {
                return OperationResult<VoteDto>.Failure($"An error occurred while casting vote: {ex.Message}");
            }
        }
    }
}
