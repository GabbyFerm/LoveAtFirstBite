﻿namespace Application.Interfaces
{
    public class RestaurantVoteCount
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public int VoteCount { get; set; }
    }

    public interface IRestaurantRepository
    {
        Task<List<RestaurantVoteCount>> GetDailyVoteTallyAsync(
            DateTime startUtc,
            DateTime endUtc,
            int round,
            CancellationToken cancellationToken = default
        );
    }
}
