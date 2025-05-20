using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RestaurantVoteDto
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int VoteCount { get; set; }
        public bool IsLeader { get; set; }
    }
}
