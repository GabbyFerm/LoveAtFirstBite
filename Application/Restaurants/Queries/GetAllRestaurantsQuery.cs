using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently

namespace Application.Restaurants.Queries
{
    public class GetAllRestaurantsQuery : IRequest<OperationResult<IEnumerable<DomainRestaurant>>> { }
}
