using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Command;
using Order.Application.DTOs;
using Order.Application.Queries;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task <IActionResult> GetAll()
        {
           var result =await _mediator.Send(new GetAllOrdersQuery());
            return Ok(ApiResponse<List<OrderDto>>.SuccessResult(result, "Orders retrieved successfully"));
        }
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _mediator.Send(new GetOrdersByUserIdQuery(userId));
            return Ok(ApiResponse<List<OrderDto>>.SuccessResult(result,"User orders retrieved successfully"));
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
           
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException(
                    "User not authenticated");

            var userId = Guid.Parse(userIdClaim);

            var command = new CreateOrderCommand( userId,request.Items
            );

            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetByUserId),new { userId },
                ApiResponse<Guid>.SuccessResult(id, "Order created successfully", 201));
        }

        [HttpPut("{id:guid}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(Guid id,[FromBody] UpdateOrderStatusCommand command)
        {
            if (id != command.Id)throw new ArgumentException("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result)throw new KeyNotFoundException($"Order with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Updated", "Order status updated successfully"));
        }
        [HttpPut("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _mediator.Send(new CancelOrderCommand(id));

            if (!result)  throw new KeyNotFoundException($"Order with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Cancelled", "Order cancelled successfully"));
        }
    }
}
