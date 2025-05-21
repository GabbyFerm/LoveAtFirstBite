using Application.Interfaces;
using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently


namespace Application.Restaurant.Queries
{
    public class GetRestaurantByIdHandler : IRequestHandler<GetRestaurantByIdQuery, OperationResult<DomainRestaurant>>
    {
        private readonly IGenericRepository<DomainRestaurant> _repository;

        public GetRestaurantByIdHandler(IGenericRepository<DomainRestaurant> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<DomainRestaurant>> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id);
        }
    }
}
