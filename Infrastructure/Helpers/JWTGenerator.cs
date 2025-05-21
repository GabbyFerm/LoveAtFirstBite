using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Helpers
{
    public class JWTGenerator : IJwtGenerator
    {
        private readonly IConfiguration _configuration;

        public JWTGenerator(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }

        public string GenerateToken(User user) 
        {
            // Retrive JWT settings from configuration
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Get the secret signing key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key missing")));

            // Define signing credentials using HMAC-SHA256 algorithm
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define claims to embed in the token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.UserEmail!),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            // Create the JWT token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            // Return the serialized token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
