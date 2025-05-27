using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using Domain.Common;

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
            var command = new CastVoteCommand(voteDto.RestaurantId, voteDto.UserId, voteDto.Round);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                var errors = result.Errors ?? new[] { result.ErrorMessage! };
                return BadRequest(errors);
            }

            return Ok(result.Data);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetTodayTally()
        {
            var result = await _mediator.Send(new GetTodayVoteTallyQuery());
            if (!result.IsSuccess)
            {
                var errors = result.Errors ?? new[] { result.ErrorMessage! };
                return BadRequest(errors);
            }
            return Ok(result.Data);
        }

        [HttpGet("today/{round}")]
        public async Task<IActionResult> GetTallyByRound(int round)
        {
            var result = await _mediator.Send(new GetTodayVoteTallyQuery(round));
            if (!result.IsSuccess)
            {
                var errors = result.Errors ?? new[] { result.ErrorMessage! };
                return BadRequest(errors);
            }
            return Ok(result.Data);
        }
    }
}
