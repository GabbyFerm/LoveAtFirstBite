using Application.Votes.Dtos;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Votes.Queries
{
    public class GetUserVoteStatusQuery : IRequest<OperationResult<UserVoteStatusDto>>
    {
        public int UserId { get; }
        public int Round { get; }

        public GetUserVoteStatusQuery(int userId, int round)
        {
            UserId = userId;
            Round = round;
        }
    }
}
