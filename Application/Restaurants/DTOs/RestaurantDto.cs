﻿
namespace Application.Restaurants.DTOs
{
    public class RestaurantDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
