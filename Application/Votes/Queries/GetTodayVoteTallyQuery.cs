using MediatR;
using Application.Votes.Dtos;
using System.Collections.Generic;

namespace Application.Votes.Queries
{
    public record GetTodayVoteTallyQuery : IRequest<List<TodayVoteTallyDto>>;
}
