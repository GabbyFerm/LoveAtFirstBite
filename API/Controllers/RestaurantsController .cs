using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Application.Authorize.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Application.Authorize.Queries;
using MediatR;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RestaurantsController(IMediator mediator) => _mediator = mediator;

        [HttpGet("vote-tally/today")]
        public async Task<ActionResult<List<RestaurantVoteDto>>> GetTodayVoteTally()
        {
            var result = await _mediator.Send(new GetTodayVoteTallyQuery());
            return Ok(result);
        }
    }
}
