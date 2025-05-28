using MediatR;
using Domain.Common;
using Application.Votes.Dtos;
using System.Collections.Generic;

namespace Application.Votes.Queries
{
    /// <summary>
    /// Fetch vote tallies for a given “today” date and specific runoff round.
    /// </summary>
    public record GetTodayVoteTallyQuery(int Round = 1)
        : IRequest<OperationResult<List<TodayVoteTallyDto>>>;
}
