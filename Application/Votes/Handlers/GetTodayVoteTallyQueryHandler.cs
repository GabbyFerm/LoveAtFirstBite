using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Application.Votes.Queries;
using Application.Votes.Dtos;
using Application.Interfaces;

namespace Application.Votes.Handlers
{
    public class GetTodayVoteTallyQueryHandler
        : IRequestHandler<GetTodayVoteTallyQuery, List<TodayVoteTallyDto>>
    {
        private readonly IRestaurantRepository _restaurantRepo;

        public GetTodayVoteTallyQueryHandler(IRestaurantRepository restaurantRepo)
            => _restaurantRepo = restaurantRepo;

        public async Task<List<TodayVoteTallyDto>> Handle(
            GetTodayVoteTallyQuery request,
            CancellationToken cancellationToken)
        {
            // 1) Compute CET “today”
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var cetNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
            var today = cetNow.Date;

            var startUtc = TimeZoneInfo.ConvertTimeToUtc(today, cetZone);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(today.AddDays(1), cetZone);

            // 2) Fetch raw vote‐counts
            var rawCounts = await _restaurantRepo
                .GetDailyVoteTallyAsync(startUtc, endUtc, cancellationToken);

            // 3) If there are no restaurants (or none returned), bail out
            if (rawCounts == null || !rawCounts.Any())
            {
                return new List<TodayVoteTallyDto>();
            }

            // 4) Determine the max for the leader flag
            var maxVotes = rawCounts.Max(x => x.VoteCount);

            // 5) Project into DTOs, sorting descending
            return rawCounts
                .OrderByDescending(x => x.VoteCount)
                .Select(x => new TodayVoteTallyDto
                {
                    RestaurantId = x.RestaurantId,
                    RestaurantName = x.RestaurantName,
                    VoteCount = x.VoteCount,
                    IsLeader = x.VoteCount == maxVotes
                })
                .ToList();
        }

    }
}