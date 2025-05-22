using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Dtos;
using Application.Votes.Queries;      

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VoteController(IMediator mediator)
            => _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> CreateVote([FromBody] VoteDto voteDto)
        {
            var command = new CastVoteCommand(voteDto.RestaurantId, voteDto.UserId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }

        [HttpGet("today")]
        public async Task<ActionResult<List<TodayVoteTallyDto>>> GetTodayTally()
        {
            var list = await _mediator.Send(new GetTodayVoteTallyQuery());
            return Ok(list);
        }
    }
}
