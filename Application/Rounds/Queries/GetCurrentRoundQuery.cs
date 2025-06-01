using Application.Rounds.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Rounds.Queries
{
    public class GetCurrentRoundQuery : IRequest<OperationResult<RoundDto>>;

}
