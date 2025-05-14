namespace Domain.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public required string RestaurantName { get; set; }
        public required string Address { get; set; }
        public int CreatedByUserId { get; set; }

        // Navigation
        public User CreatedByUser { get; set; } = null!;
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
