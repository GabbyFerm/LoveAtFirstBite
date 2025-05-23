using Domain.Common;
using MediatR;

namespace Application.Votes.Commands.ResetVotes
{
    public class ResetVotesCommand : IRequest<OperationResult<bool>> { }
}
