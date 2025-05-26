using Application.Restaurants.Commands;
using FluentValidation;

namespace Application.Restaurants.Validator
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        public CreateRestaurantCommandValidator()
        {
            RuleFor(x => x.RestaurantName)
                .NotEmpty().WithMessage("Restaurant name is required.")
                .MaximumLength(100).WithMessage("Restaurant name cannot exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
        }
    }
}
