using MediatR;
using Domain.Common;
using Application.Votes.Dtos;
using System.Collections.Generic;

namespace Application.Votes.Queries
{
    public record GetTodayVoteTallyQuery
        : IRequest<OperationResult<List<TodayVoteTallyDto>>>;
}
