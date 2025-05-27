using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Commands.CreeateVote;
using Application.Authorize.DTOs;
using Application.Votes.Dtos;
using Domain.Common;
using System.Security.Claims;
using Application.Votes.Commands.ResetVotes;
using Microsoft.AspNetCore.Authorization;
using Application.Votes.Commands.ChangeVote;

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
        [Authorize]
        public async Task<IActionResult> CreateVote([FromBody] VoteDto voteDto)
        {
            // Extract the user ID from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("Invalid user ID in token.");

            // Create command with userId from token
            var command = new CastVoteCommand(voteDto.RestaurantId, userId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result);
        }






        [HttpPut("Change")]
        [Authorize]
        public async Task<IActionResult> ChangeVote([FromBody] VoteDto voteDto)
        {
            // Validate restaurantId
            if (voteDto.RestaurantId <= 0)
                return BadRequest("Invalid restaurant ID.");

            // Extract user ID from JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized("User ID not found or invalid in token.");

            // Create and send command
            var command = new ChangeVoteCommand(voteDto.RestaurantId, userId);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);



            return Ok(new
            {
                vote = result.Data
            });
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