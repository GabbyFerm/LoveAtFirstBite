using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly LoveAtFirstBiteDbContext _db;

        public RestaurantRepository(LoveAtFirstBiteDbContext db)
            => _db = db;

        public async Task<List<RestaurantVoteCount>> GetDailyVoteTallyAsync(
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken cancellationToken = default)
        {
            return await _db.Restaurants
                .Select(r => new RestaurantVoteCount
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    VoteCount = r.Votes.Count(v =>
                        v.VoteDate >= startUtc &&
                        v.VoteDate < endUtc)
                })
                .ToListAsync(cancellationToken);
        }
    }
}
