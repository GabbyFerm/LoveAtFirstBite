using Application.Interfaces;
using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently


namespace Application.Restaurant.Commands
{
    public class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, OperationResult<DomainRestaurant>>
    {
        private readonly IGenericRepository<DomainRestaurant> _repository;

        public CreateRestaurantHandler(IGenericRepository<DomainRestaurant> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<DomainRestaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = new DomainRestaurant
            {
                RestaurantName = request.RestaurantName,
                Address = request.Address,
                CreatedByUserId = request.CreatedByUserId
            };

            return await _repository.AddAsync(restaurant);
        }
    }
}
