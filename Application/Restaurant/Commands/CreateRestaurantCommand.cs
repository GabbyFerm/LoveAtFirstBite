using Domain.Common;
using MediatR;
using Domain.Models;

namespace Application.Restaurant.Commands
{
    public class CreateRestaurantCommand : IRequest<OperationResult<Domain.Models.Restaurant>>
    {
        public string? RestaurantName { get; set; }
        public string? Address { get; set; }
        public int CreatedByUserId { get; set; }

    }
}
