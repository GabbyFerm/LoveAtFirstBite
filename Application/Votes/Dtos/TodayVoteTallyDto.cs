namespace Application.Votes.Dtos
{
    /// <summary>
    /// A daily snapshot of how many votes each restaurant got, 
    /// plus a flag for the top‐voted ones.
    /// </summary>
    public class TodayVoteTallyDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public int VoteCount { get; set; }
        public bool IsLeader { get; set; }
    }
}

