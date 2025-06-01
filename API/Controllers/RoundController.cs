using Application.Rounds.Commands;
using Application.Rounds.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoundController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrent()
        {
            var result = await _mediator.Send(new GetCurrentRoundQuery());

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Increment()
        {
            var result = await _mediator.Send(new IncrementRoundCommand());

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }

        [HttpPut("reset")]
        [Authorize]
        public async Task<IActionResult> Reset()
        {
            var result = await _mediator.Send(new ResetRoundCommand());

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Data);
        }
    }
}
