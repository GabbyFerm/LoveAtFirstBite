using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently


namespace Application.Restaurant.Commands
{
    public class UpdateRestaurantCommand : IRequest<OperationResult<DomainRestaurant>>
    {
        public int Id { get; set; }
        public string? RestaurantName { get; set; }
        public string? Address { get; set; }
        public string? UpdatedByUserId { get; set; }
    }
}
