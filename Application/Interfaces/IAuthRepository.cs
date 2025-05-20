using Domain.Models;

namespace Application.Interfaces
{
    public interface IAuthRepository
    {
        // Retrive a user by their username
        Task<User?> GetUserByUsernameAsync(string username);

        // Retrive a user by their email
        Task<User?> GetUserByEmailAsync(string email);

        // Checks if email is already registered in the system
        Task<bool> EmailExistsAsync(string email);

        // Adds a new user to the database (registration)
        Task CreateUserAsync(User user);

        // Saves pending changes to the database and wraps the result
        //Task<OperationResult<bool>> SaveChangesAsync();
    }
}
