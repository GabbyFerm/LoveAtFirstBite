using Application.Interfaces;
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
            return await _context.Users.AnyAsync(user => user.UserEmail.ToLower() == email.ToLower());
        }

        // Get a user by email (used in password reset)
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserEmail.ToLower() == email.ToLower());
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName.ToLower() == username.ToLower());
        }
    }
}
