using Application.Authorize.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator() 
        {
            // Username must be provided and withing valid length
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Must(userName => !string.IsNullOrWhiteSpace(userName)).WithMessage("Username cannot be blank.")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters.");

            // Email must be valid and not empty
            RuleFor(user => user.UserEmail)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            // Password must be strong and meet requirements
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.");
        }
    }
}
