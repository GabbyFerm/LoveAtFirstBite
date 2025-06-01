using Application.Rounds.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Rounds.Commands
{
    public class IncrementRoundCommand : IRequest<OperationResult<RoundDto>>;
}
