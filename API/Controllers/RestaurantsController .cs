using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly LoveAtFirstBiteDbContext _context;

        // Dependency injection of the database context
        public RestaurantsController(LoveAtFirstBiteDbContext context)
        {
            _context = context;
        }

        // GET /api/restaurants/vote-tally/today
        [HttpGet("vote-tally/today")]
        public async Task<IActionResult> GetTodayVoteTally()
        {
            // Determine the current date in Sweden's local timezone (Central European Time)
            TimeZoneInfo swedenTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            DateTime nowInSweden = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, swedenTimeZone);
            DateTime todaySweden = nowInSweden.Date;                // Midnight of today in CET
            DateTime tomorrowSweden = todaySweden.AddDays(1);       // Midnight of the next day in CET

            // Query all restaurants with their vote count for today (left join via navigation property count)
            var restaurantVotes = await _context.Restaurants
                .Select(r => new RestaurantVoteDto
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    Address = r.Address,
                    // Count votes for this restaurant where VoteDate is between today and tomorrow in CET
                    VoteCount = r.Votes.Count(v => v.VoteDate >= todaySweden && v.VoteDate < tomorrowSweden)
                })
                .OrderByDescending(dto => dto.VoteCount)  // sort results by vote count (highest first)
                .ToListAsync();

            // Determine the highest vote count (if any) to mark leaders
            int maxVotes = restaurantVotes.Any() ? restaurantVotes.Max(dto => dto.VoteCount) : 0;
            foreach (var dto in restaurantVotes)
            {
                // IsLeader if this restaurant's count equals the max count (ties are possible)
                dto.IsLeader = (dto.VoteCount == maxVotes && maxVotes > 0);
            }

            return Ok(restaurantVotes);
        }
    }
}
