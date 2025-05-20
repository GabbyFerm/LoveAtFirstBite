using Application.Authorize.Queries.Login;
using FluentValidation;

namespace Application.Validators
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(user => user.UserName).NotEmpty().WithMessage("Username is required.");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}
