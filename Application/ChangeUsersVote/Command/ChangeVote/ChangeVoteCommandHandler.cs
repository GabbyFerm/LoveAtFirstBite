using Application.Interfaces;
using Application.Votes.Dtos;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Application.Votes.Commands.ChangeVote
{
    public class ChangeVoteCommandHandler : IRequestHandler<ChangeVoteCommand, OperationResult<VoteDto>>
    {
        private readonly IGenericRepository<Vote> _voteRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;

        public ChangeVoteCommandHandler(
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

        public async Task<OperationResult<VoteDto>> Handle(ChangeVoteCommand request, CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // Fetch today's vote
            var existingVote = await _voteRepository
                .AsQueryable()
                .FirstOrDefaultAsync(v => v.UserId == request.UserId && v.VoteDate == today, cancellationToken);

            if (existingVote == null)
                return OperationResult<VoteDto>.Failure("No vote found for today to change.");

            if (existingVote.RestaurantId == request.RestaurantId)
                return OperationResult<VoteDto>.Failure("You have already voted for this restaurant today.");

            // Check if the new restaurant is valid
            var restaurantCheck = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (!restaurantCheck.IsSuccess)
                return OperationResult<VoteDto>.Failure("Invalid restaurant selected.");

            // Update restaurant in vote
            existingVote.RestaurantId = request.RestaurantId;
            await _voteRepository.UpdateAsync(existingVote, cancellationToken);

            var updatedVoteDto = _mapper.Map<VoteDto>(existingVote);

            return new OperationResult<VoteDto>
            {
                Data = updatedVoteDto,
                IsSuccess = true,
            };
        }
    }
}
