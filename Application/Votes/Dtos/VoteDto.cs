namespace Application.Votes.Dtos
{
    public class VoteDto
    {
        public int RestaurantId { get; set; }
        public DateTime VoteDate { get; set; } // Optional but helpful for returning data
        public int Round { get; set; }
    }
}
