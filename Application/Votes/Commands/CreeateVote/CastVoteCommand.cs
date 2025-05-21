using Application.Votes.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Votes.Commands.CreeateVote
{
    public class CastVoteCommand : IRequest<OperationResult<VoteDto>>
    {
        public CastVoteCommand(int restaurantId, int userId)
        {
            RestaurantId = restaurantId;
            UserId = userId;
        }

        public int RestaurantId { get; set; }
        public int UserId { get; set; }
       
    }
}
