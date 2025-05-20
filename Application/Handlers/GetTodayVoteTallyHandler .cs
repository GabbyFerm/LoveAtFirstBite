// Application/Handlers/GetTodayVoteTallyHandler.cs
using Application.Authorize.DTOs;
using Application.Authorize.Queries;
using Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetTodayVoteTallyHandler
        : IRequestHandler<GetTodayVoteTallyQuery, List<RestaurantVoteDto>>
    {
        private readonly IRestaurantRepository _repo;

        public GetTodayVoteTallyHandler(IRestaurantRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<RestaurantVoteDto>> Handle(
            GetTodayVoteTallyQuery request,
            CancellationToken cancellationToken)
        {
            return await _repo.GetTodayVoteTallyAsync();
        }
    }
}
