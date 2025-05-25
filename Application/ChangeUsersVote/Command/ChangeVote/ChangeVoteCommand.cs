using Application.Votes.Dtos;
using Domain.Common;
using MediatR;

namespace Application.Votes.Commands.ChangeVote
{
    public class ChangeVoteCommand : IRequest<OperationResult<VoteDto>>
    {
     
        public int RestaurantId { get; }
        public int UserId { get; }

        public ChangeVoteCommand(int restaurantId, int userId)
        {
           
            RestaurantId = restaurantId;
            UserId = userId;
        }
    }
}

