using Application.Interfaces;
using Application.Restaurants.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;

namespace Application.Restaurants.Commands
{
    public class CreateRestaurantHandler : IRequestHandler<CreateRestaurantCommand, OperationResult<CreateRestaurantDto>>
    {
        private readonly IGenericRepository<Restaurant> _repository;
        private readonly IMapper _mapper;

        public CreateRestaurantHandler(IGenericRepository<Restaurant> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<CreateRestaurantDto>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
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
                    return OperationResult<CreateRestaurantDto>.Failure(result.ErrorMessage!);

                var dto = _mapper.Map<CreateRestaurantDto>(result.Data);
                return OperationResult<CreateRestaurantDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<CreateRestaurantDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
