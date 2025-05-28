using Application.Restaurants.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Restaurants.Queries
{
    public class GetAllRestaurantsQuery : IRequest<OperationResult<IEnumerable<RestaurantDto>>>
    {
    }
}
