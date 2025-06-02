namespace Application.Votes.Dtos
{
    public class VoteRecordDto
    {
        public int VoteId { get; set; }
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public DateTime VoteDate { get; set; }
        public int Round {  get; set; }
    }
}
