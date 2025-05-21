using Application.Authorize.Commands.Register;
using Application.Authorize.DTOs;
using Application.Restaurants.Commands;
using Application.Restaurants.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RestaurantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        //{
        //    var command = new RegisterCommand(userRegisterDto.UserName, userRegisterDto.UserEmail, userRegisterDto.Password);
        //    var result = await _mediator.Send(command);

        //    if (!result.IsSuccess)
        //        return BadRequest(result);

        //    return Ok(result);
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command) // change into DTOS here
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRestaurantsQuery());
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetRestaurantByIdQuery(id));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantCommand command)
        {
            if (id != command.Id) return BadRequest("Mismatched IDs.");
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRestaurantCommand(id));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
