using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Commands.CreeateVote;
using Application.Authorize.DTOs;
using Application.Votes.Dtos;
using Domain.Common;
using Application.Votes.Queries;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VoteController(IMediator mediator)
        {

            _mediator = mediator;

        }


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
        public async Task<IActionResult> GetTodayTally()
        {
            OperationResult<List<TodayVoteTallyDto>> result
                = await _mediator.Send(new GetTodayVoteTallyQuery());

            if (!result.IsSuccess)
            {
                var errors = result.Errors ?? new[] { result.ErrorMessage! };
                return BadRequest(errors);
            }

            return Ok(result.Data);

        }
    }
}