using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using Domain.Common;

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
        public async Task<IActionResult> Get()
        {
            OperationResult<List<TodayVoteTallyDto>> result =
                await _mediator.Send(new GetTodayVoteTallyQuery());

            if (!result.IsSuccess)
            {
                var errors = result.Errors ?? new[] { result.ErrorMessage! };
                return BadRequest(errors);
            }

            return Ok(result.Data);
        }
    }
}
