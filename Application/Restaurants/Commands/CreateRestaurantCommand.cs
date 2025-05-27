using Domain.Common;
using MediatR;
using Application.Restaurants.DTOs;
using System.Text.Json.Serialization;

namespace Application.Restaurants.Commands
{
    public class CreateRestaurantCommand : IRequest<OperationResult<CreateRestaurantDto>>
    {

        public string? RestaurantName { get; set; }
        public string? Address { get; set; }
        [JsonIgnore]
        public int CreatedByUserId { get; set; }
    }
}
