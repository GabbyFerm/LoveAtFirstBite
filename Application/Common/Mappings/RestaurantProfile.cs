using DomainRestaurant = Domain.Models.Restaurant; // should be solved differently
using Application.Restaurants.DTOs;
using AutoMapper;
using Domain.Models;

namespace Application.Common.Mappings
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<DomainRestaurant, RestaurantDto>();
            CreateMap<RestaurantDto, DomainRestaurant>();
        }
    }
}
