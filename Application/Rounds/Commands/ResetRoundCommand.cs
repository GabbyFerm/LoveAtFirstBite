using Domain.Common;
using MediatR;

namespace Application.Rounds.Commands
{
    public class ResetRoundCommand : IRequest<OperationResult<int>> { }
}
