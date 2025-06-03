using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            int round,
            CancellationToken cancellationToken = default)
        {
            return await _db.Restaurants
                .Select(r => new RestaurantVoteCount
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    VoteCount = r.Votes.Count(v =>
                        v.VoteDate >= startUtc &&
                        v.VoteDate < endUtc &&
                        v.Round == round)
                })
                .ToListAsync(cancellationToken);
        }
    }
}
