using Application.Interfaces;
using Domain.Common;
using MediatR;
using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently


namespace Application.Restaurant.Commands
{
    public class DeleteRestaurantHandler : IRequestHandler<DeleteRestaurantCommand, OperationResult<bool>>
    {
        private readonly IGenericRepository<DomainRestaurant> _repository;

        public DeleteRestaurantHandler(IGenericRepository<DomainRestaurant> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteByIdAsync(request.Id);
        }
    }
}
