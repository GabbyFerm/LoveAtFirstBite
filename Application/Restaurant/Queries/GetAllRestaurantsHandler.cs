using Application.Interfaces;
using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently

namespace Application.Restaurant.Queries
{
    public class GetAllRestaurantsHandler : IRequestHandler<GetAllRestaurantsQuery, OperationResult<IEnumerable<DomainRestaurant>>>
    {
        private readonly IGenericRepository<DomainRestaurant> _repository;

        public GetAllRestaurantsHandler(IGenericRepository<DomainRestaurant> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<IEnumerable<DomainRestaurant>>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
