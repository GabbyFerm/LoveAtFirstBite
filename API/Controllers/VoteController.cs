﻿using Application.Votes.Commands.ChangeVote;
using Application.Votes.Commands.CreeateVote;
using Application.Votes.Commands.ResetVotes;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("all-votes")]
        [Authorize]
        public async Task<IActionResult> GetAllVotes() 
        { 
          var result = await _mediator.Send(new GetAllVotesQuery());

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result);
        }
    }
}