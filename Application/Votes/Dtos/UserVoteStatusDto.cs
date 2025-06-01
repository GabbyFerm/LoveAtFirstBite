using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Votes.Dtos
{
    public class UserVoteStatusDto
    {
        public bool HasVoted { get; set; }
        public int? VotedRestaurantId { get; set; }
    }
}
