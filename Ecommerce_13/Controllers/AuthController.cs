using Ecommerce_13.Comman;
using Identity.Application.Command;
using Identity.Application.DTOs;
using Identity.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private  readonly  IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = new RegisterUserCommand(
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.Password,
                dto.Role
            );

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<AuthResponseDto>.SuccessResult(result, "User registered successfully"));
        }
     
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = new LoginUserCommand(dto.Email,dto.Password);

            var result = await _mediator.Send(command);
            return Ok(ApiResponse<AuthResponseDto>.SuccessResult(result, "Login successful"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUsersQuery(ActiveOnly: true));

            return Ok(ApiResponse<List<UserDto>>.SuccessResult(result, "Active users retrieved successfully"));
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllIncludingDeleted()
        {
            var result = await _mediator.Send(new GetAllUsersQuery(ActiveOnly: false));

            return Ok(ApiResponse<List<UserDto>>.SuccessResult(result, "All users retrieved successfully"));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id,[FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                throw new ArgumentException("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result)throw new KeyNotFoundException($"User with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Updated", "User updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));

            if (!result) throw new KeyNotFoundException( $"User with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Deleted", "User deactivated successfully"));
        }

    }
}
