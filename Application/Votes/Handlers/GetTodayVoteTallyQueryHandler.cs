﻿using Application.Interfaces;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using Domain.Common;
using MediatR;

namespace Application.Votes.Handlers
{
    public class GetTodayVoteTallyQueryHandler
        : IRequestHandler<GetTodayVoteTallyQuery, OperationResult<List<TodayVoteTallyDto>>>
    {
        private readonly IRestaurantRepository _restaurantRepo;

        public GetTodayVoteTallyQueryHandler(IRestaurantRepository restaurantRepo)
            => _restaurantRepo = restaurantRepo;

        public async Task<OperationResult<List<TodayVoteTallyDto>>> Handle(
            GetTodayVoteTallyQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                // Compute CET “today”
                var cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var cetNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);
                var today = cetNow.Date;
                var startUtc = TimeZoneInfo.ConvertTimeToUtc(today, cetZone);
                var endUtc = TimeZoneInfo.ConvertTimeToUtc(today.AddDays(1), cetZone);

                // Fetch counts at the requested round
                var rawCounts = await _restaurantRepo
                    .GetDailyVoteTallyAsync(startUtc, endUtc, request.Round, cancellationToken);

                if (rawCounts == null || !rawCounts.Any())
                    return OperationResult<List<TodayVoteTallyDto>>.Failure("No votes today");

                var maxVotes = rawCounts.Max(x => x.VoteCount);

                var list = rawCounts
                    .OrderByDescending(x => x.VoteCount)
                    .Select(x => new TodayVoteTallyDto
                    {
                        RestaurantId = x.RestaurantId,
                        RestaurantName = x.RestaurantName,
                        VoteCount = x.VoteCount,
                        IsLeader = x.VoteCount == maxVotes
                    })
                    .ToList();

                return OperationResult<List<TodayVoteTallyDto>>.Success(list);
            }
            catch (Exception)
            {
                return OperationResult<List<TodayVoteTallyDto>>
                    .Failure("Something broke while fetching today’s tally");
            }
        }
    }
}
