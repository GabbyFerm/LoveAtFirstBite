using Application.Votes.Commands.ChangeVote;
using Application.Votes.Dtos;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangeVoteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ChangeVoteController(IMediator mediator)
        {
            _mediator = mediator; 
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
                message = result.Message ?? "Vote updated successfully.",
                vote = result.Data
            });
        }
    }
}
