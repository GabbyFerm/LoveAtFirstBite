using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Dtos;
using Application.Votes.Queries;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodayVoteTallyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodayVoteTallyController(IMediator mediator)
            => _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<List<TodayVoteTallyDto>>> Get()
        {
            var result = await _mediator.Send(new GetTodayVoteTallyQuery());
            return Ok(result);
        }
    }
}
