using Application.Interfaces;
using Domain.Common;
using Domain.Models;
using BCrypt.Net;
using MediatR;
using AutoMapper;

namespace Application.Authorize.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OperationResult<string>>
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAuthRepository authRepository, IJwtGenerator jwtGenerator, IMapper mapper)
        {
            _authRepository = authRepository;
            _jwtGenerator = jwtGenerator;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Normalize the email to lowercase once
                var normalizedEmail = request.UserEmail.ToLower();

                // Check if the email is already registered
                var emailExists = await _authRepository.EmailExistsAsync(normalizedEmail);
                if (emailExists)
                    return OperationResult<string>.Failure("Email is already registered.");

                // Map the DTO to a User entity
                var user = _mapper.Map<User>(request);

                // Hash the password manually
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.UserEmail = user.UserEmail.ToLower();

                // Add the user to the context (not saved yet)
                await _authRepository.CreateUserAsync(user);

                // Try to generate the token (if this fails, no DB changes happen)
                var token = _jwtGenerator.GenerateToken(user);

                // Save the user only if everything succeeded
                await _authRepository.SaveChangesAsync();

                // Return token
                return OperationResult<string>.Success(token);
            }
            catch (Exception ex) 
            {
                // Handle unexpected errors
                return OperationResult<string>.Failure($"Error during registration: {ex.Message}");
            }
        }
    }
}
