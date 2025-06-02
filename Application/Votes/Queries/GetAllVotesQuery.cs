using Application.Votes.Dtos;
using Domain.Common;
using MediatR;

namespace Application.Votes.Queries
{
    public class GetAllVotesQuery : IRequest<OperationResult<IEnumerable<VoteRecordDto>>>
    {
    }
}
