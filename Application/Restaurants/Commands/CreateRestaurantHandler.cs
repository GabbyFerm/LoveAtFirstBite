using Application.Interfaces;
using Domain.Common;
using Domain.Models;
using MediatR;


namespace Application.Restaurants.Commands
{
    public class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, OperationResult<Restaurant>>
    {
        private readonly IGenericRepository<Restaurant> _repository;

        public CreateRestaurantHandler(IGenericRepository<Restaurant> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var restaurant = new Restaurant
            {
                RestaurantName = request.RestaurantName!,
                Address = request.Address!,
                CreatedByUserId = request.CreatedByUserId
            };
            return await _repository.AddAsync(restaurant);
        }
    }
}
