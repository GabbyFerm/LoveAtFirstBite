using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    /// <summary>
    /// Simple DTO for repository projections.
    /// </summary>
    public class RestaurantVoteCount
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public int VoteCount { get; set; }
    }

    public interface IRestaurantRepository
    {
        /// <summary>
        /// Returns, for each restaurant, how many votes it received within [startUtc, endUtc).
        /// Includes restaurants with zero votes.
        /// </summary>
        Task<List<RestaurantVoteCount>> GetDailyVoteTallyAsync(
            DateTime startUtc,
            DateTime endUtc,
            CancellationToken cancellationToken = default
        );
    }
}
