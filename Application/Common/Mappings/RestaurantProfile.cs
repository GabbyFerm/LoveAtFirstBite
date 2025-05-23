using Domain.Models;
using Application.Restaurants.DTOs;
using AutoMapper;

namespace Application.Common.Mappings
{
    public class RestaurantProfile : Profile
    {
        public RestaurantProfile()
        {
            CreateMap<RestaurantDto, Restaurant>();
            CreateMap<Restaurant, RestaurantDto>();
        }
    }
}
