using Application.Votes.Dtos;
using Domain.Common;
using MediatR;

namespace Application.Votes.Commands.CreeateVote
{
    public class CastVoteCommand : IRequest<OperationResult<VoteDto>>
    {
        // Constructor with optional round (defaults to 1)
        public CastVoteCommand(int restaurantId, int userId, int round = 1)
        {
            RestaurantId = restaurantId;
            UserId = userId;
            Round = round;
        }

        public int RestaurantId { get; set; }
        public int UserId { get; set; }

        // New: which round this vote belongs to
        public int Round { get; set; } = 1;
    }
}
