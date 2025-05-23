using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Votes.Dtos
{
    public class VoteDto
    {
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public DateTime VoteDate { get; set; } // Optional but helpful for returning data
        public int Round { get; set; }
    }
}
