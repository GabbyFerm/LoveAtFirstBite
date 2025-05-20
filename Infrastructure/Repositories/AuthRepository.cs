using Application.Interfaces;
using Domain.Common;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly LoveAtFirstBiteDbContext _context;

        public AuthRepository(LoveAtFirstBiteDbContext context)
        {
            _context = context;
        }

        // Add a new user to the database (save is called separately)
        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        // Check if email is already registered
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(user => user.UserEmail!.ToLower() == email.ToLower());
        }

        // Get user by username (used in login)
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName!.ToLower() == username.ToLower());
        }

        // Save changes to database
        public async Task<OperationResult<bool>> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return OperationResult<bool>.Failure($"Saving changes failed: {ex.Message}");
            }
        }
    }
}
