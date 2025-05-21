using Application.Authorize.Commands.Register;
using Application.Authorize.DTOs;
using Application.Authorize.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto) 
        { 
            var command = new RegisterCommand(userRegisterDto.UserName, userRegisterDto.UserEmail, userRegisterDto.Password);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess) 
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto) 
        { 
            var query = new LoginQuery(userLoginDto.UserName, userLoginDto.Password);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
