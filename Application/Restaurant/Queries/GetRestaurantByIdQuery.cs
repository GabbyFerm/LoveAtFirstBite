using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently


namespace Application.Restaurant.Queries
{
    public class GetRestaurantByIdQuery : IRequest<OperationResult<DomainRestaurant>>
    {
        public int Id { get; }

        public GetRestaurantByIdQuery(int id)
        {
            Id = id;
        }
    }
}
