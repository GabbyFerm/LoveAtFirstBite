namespace Domain.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public DateTime VoteDate {  get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
        public int Round { get; set; } = 1;    // new!

    }
}
