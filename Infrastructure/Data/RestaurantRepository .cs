using Application.Authorize.DTOs;
using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly LoveAtFirstBiteDbContext _db;
        private readonly TimeZoneInfo _swedenTz;

        public RestaurantRepository(LoveAtFirstBiteDbContext db)
        {
            _db = db;
            _swedenTz = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        }

        public async Task<List<RestaurantVoteDto>> GetTodayVoteTallyAsync()
        {
            // 1. Determine "today" in Sweden CET
            var nowUtc = DateTime.UtcNow;
            var nowInSweden = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, _swedenTz);
            var todayCETStart = nowInSweden.Date;
            var tomorrowCETStart = todayCETStart.AddDays(1);

            // 2. Convert CET boundaries back to UTC for filtering stored UTC VoteDate
            var utcStart = TimeZoneInfo.ConvertTimeToUtc(todayCETStart, _swedenTz);
            var utcEnd = TimeZoneInfo.ConvertTimeToUtc(tomorrowCETStart, _swedenTz);

            // 3. Query vote counts using UTC boundaries
            var list = await _db.Restaurants
                .Select(r => new RestaurantVoteDto
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    Address = r.Address,
                    VoteCount = r.Votes.Count(v =>
                        v.VoteDate >= utcStart &&
                        v.VoteDate < utcEnd)
                })
                .OrderByDescending(x => x.VoteCount)
                .ToListAsync();

            // 4. Flag leaders
            var maxVotes = list.Any() ? list.Max(x => x.VoteCount) : 0;
            foreach (var dto in list)
            {
                dto.IsLeader = dto.VoteCount == maxVotes && maxVotes > 0;
            }

            return list;
        }
    }
}
