using Application.Commen.Dtos;
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
        public int VoteId { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
