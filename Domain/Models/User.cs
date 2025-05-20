namespace Domain.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string PasswordHash { get; set; } = null!;

        // Navigation
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}
