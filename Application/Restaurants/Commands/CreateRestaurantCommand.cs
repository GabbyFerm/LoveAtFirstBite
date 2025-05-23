using Domain.Common;
using MediatR;
using Application.Restaurants.DTOs;

namespace Application.Restaurants.Commands
{
    public class CreateRestaurantCommand : IRequest<OperationResult<RestaurantDto>>
    {

        public string? RestaurantName { get; set; }
        public string? Address { get; set; }
        public int CreatedByUserId { get; set; }

        public CreateRestaurantCommand(string restaurantName, string address, int createdByUserId)
        {
            RestaurantName = restaurantName;
            Address = address;
            CreatedByUserId = createdByUserId;
        }
    }
}
