namespace Domain.Models
{
    public class Round
    {
        public int RoundId { get; set; } // primary key
        public int CurrentRound { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
