using Domain.Models;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        // Generates a JWT token for the authenticated user
        string GenerateToken(User user);
    }
}
