using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Authorize.DTOs;
using MediatR;

namespace Application.Authorize.Queries
{
    public class GetTodayVoteTallyQuery : IRequest<List<RestaurantVoteDto>> { }
}
