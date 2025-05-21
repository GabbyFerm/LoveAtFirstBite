using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Restaurants.DTOs
{
    public class RestaurantDto
    {
        public required string RestaurantName { get; set; }
        public required string Address { get; set; }
    }
}
