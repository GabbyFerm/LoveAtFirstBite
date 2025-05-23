using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Commands.CreeateVote;
using Application.Authorize.DTOs;
using Application.Votes.Dtos;
using Domain.Common;
using Application.Votes.Commands.ResetVotes;
using Microsoft.AspNetCore.Authorization;

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

        [HttpDelete("reset")]
        [Authorize]
        public async Task<IActionResult> ResetVotes()
        {
            var result = await _mediator.Send(new ResetVotesCommand());

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(new { message = "Votes reset", deleted = result.Data });
        }

    }
}