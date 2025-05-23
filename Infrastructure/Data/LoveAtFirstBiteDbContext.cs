using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class LoveAtFirstBiteDbContext : DbContext
    {
        public LoveAtFirstBiteDbContext(DbContextOptions<LoveAtFirstBiteDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User → Restaurant (1-to-many)
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.CreatedByUser)
                .WithMany(u => u.Restaurants)
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Vote (1-to-many)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Restaurant → Vote (1-to-many)
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Restaurant)
                .WithMany(r => r.Votes)
                .HasForeignKey(v => v.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.VoteDate })
                .IsUnique(); // Ensure one vote per user per day

            modelBuilder.Entity<Vote>()
                .Property(v => v.Round)
                .HasDefaultValue(1)       // SQL default = 1
                .ValueGeneratedOnAdd();
        }
    }
}
