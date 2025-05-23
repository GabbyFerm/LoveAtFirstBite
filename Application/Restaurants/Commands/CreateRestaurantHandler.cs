using Application.Interfaces;
using Application.Restaurants.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;


namespace Application.Restaurants.Commands
{
    public class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, OperationResult<RestaurantDto>>
    {
        private readonly IGenericRepository<Restaurant> _repository;
        private readonly IMapper _mapper;

        public CreateRestaurantHandler(IGenericRepository<Restaurant> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<RestaurantDto>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var restaurant = new Restaurant
                {
                    RestaurantName = request.RestaurantName!,
                    Address = request.Address!,
                    CreatedByUserId = request.CreatedByUserId
                };

                var result = await _repository.AddAsync(restaurant);

                if (!result.IsSuccess)
                    return OperationResult<RestaurantDto>.Failure(result.ErrorMessage!);

                var dto = _mapper.Map<RestaurantDto>(result.Data);
                return OperationResult<RestaurantDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<RestaurantDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
