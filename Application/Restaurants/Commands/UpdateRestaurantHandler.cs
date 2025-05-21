using Application.Interfaces;
using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently


namespace Application.Restaurants.Commands
{
    public class UpdateRestaurantHandler : IRequestHandler<UpdateRestaurantCommand, OperationResult<DomainRestaurant>>
    {
        private readonly IGenericRepository<DomainRestaurant> _repository;

        public UpdateRestaurantHandler(IGenericRepository<DomainRestaurant> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<DomainRestaurant>> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);

            if (!existing.IsSuccess || existing.Data == null)
                return OperationResult<DomainRestaurant>.Failure("Restaurant not found.");

            var restaurant = existing.Data;
            restaurant.RestaurantName = request.RestaurantName;
            restaurant.Address = request.Address;

            return await _repository.UpdateAsync(restaurant);
        }
    }

}
