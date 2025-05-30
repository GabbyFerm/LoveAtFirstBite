using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Common;
using Domain.Models;
using Application.Interfaces;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using AutoMapper;

namespace Application.Votes.Commands.CreeateVote
{
    public class CastVoteCommandHandler
        : IRequestHandler<CastVoteCommand, OperationResult<VoteDto>>
    {
        private readonly IGenericRepository<Vote> _voteRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Restaurant> _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CastVoteCommandHandler(
            IGenericRepository<Vote> voteRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Restaurant> restaurantRepository,
            IMapper mapper,
            IMediator mediator
        )
        {
            _voteRepository = voteRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<OperationResult<VoteDto>> Handle(
            CastVoteCommand request,
            CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.Date;

            // Tie-break enforcement: only allow voting on tied restaurants in later rounds
            if (request.Round > 1)
            {
                var prevResult = await _mediator.Send(
                    new GetTodayVoteTallyQuery(request.Round - 1), cancellationToken);
                if (!prevResult.IsSuccess)
                    return OperationResult<VoteDto>.Failure("Cannot perform tie-break: failed to fetch previous round.");

                var tiedIds = prevResult.Data!
                    .Where(x => x.IsLeader)
                    .Select(x => x.RestaurantId)
                    .ToHashSet();

                if (!tiedIds.Contains(request.RestaurantId))
                    return OperationResult<VoteDto>.Failure(
                        "You can only vote among the tied restaurants for this round.");
            }

            // Validate user
            var userResult = await _userRepository.GetByIdAsync(request.UserId);
            if (!userResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("User not found.");

            // Validate restaurant
            var restResult = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (!restResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("Restaurant not found.");

            // Check existing vote for this round (synchronous)
            var existingVote = _voteRepository.AsQueryable()
                .FirstOrDefault(v =>
                    v.UserId == request.UserId &&
                    v.VoteDate == today &&
                    v.Round == request.Round);

            if (existingVote != null)
            {
                if (existingVote.RestaurantId == request.RestaurantId)
                    return OperationResult<VoteDto>.Failure(
                        "You’ve already voted for this restaurant this round.");

                existingVote.RestaurantId = request.RestaurantId;
                var updateResult = await _voteRepository.UpdateAsync(existingVote, cancellationToken);
                if (!updateResult.IsSuccess)
                    return OperationResult<VoteDto>.Failure("Failed to update vote.");

                return OperationResult<VoteDto>.Success(_mapper.Map<VoteDto>(existingVote));
            }

            // Create new vote
            var newVote = new Vote
            {
                UserId = request.UserId,
                RestaurantId = request.RestaurantId,
                VoteDate = today,
                Round = request.Round
            };

            var createResult = await _voteRepository.AddAsync(newVote, cancellationToken);
            if (!createResult.IsSuccess)
                return OperationResult<VoteDto>.Failure("Failed to create vote.");

            return OperationResult<VoteDto>.Success(_mapper.Map<VoteDto>(createResult.Data!));
        }
    }
}
