using Application.Interfaces;
using Application.Restaurants.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;

namespace Application.Restaurants.Queries
{
    public class GetAllRestaurantsHandler : IRequestHandler<GetAllRestaurantsQuery, OperationResult<IEnumerable<RestaurantDto>>>
    {
        private readonly IGenericRepository<Restaurant> _repository;
        private readonly IMapper _mapper;

        public GetAllRestaurantsHandler(IGenericRepository<Restaurant> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<RestaurantDto>>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetAllAsync();

                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<RestaurantDto>>.Failure(result.ErrorMessage!);

                var mapped = _mapper.Map<IEnumerable<RestaurantDto>>(result.Data);
                return OperationResult<IEnumerable<RestaurantDto>>.Success(mapped);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<RestaurantDto>>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
