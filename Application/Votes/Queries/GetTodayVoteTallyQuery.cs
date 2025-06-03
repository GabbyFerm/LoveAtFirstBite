using Application.Votes.Dtos;
using Domain.Common;
using MediatR;

namespace Application.Votes.Queries
{
    /// <summary>
    /// Fetch vote tallies for a given “today” date and specific runoff round.
    /// </summary>
    public record GetTodayVoteTallyQuery(int Round = 1)
        : IRequest<OperationResult<List<TodayVoteTallyDto>>>;
}
