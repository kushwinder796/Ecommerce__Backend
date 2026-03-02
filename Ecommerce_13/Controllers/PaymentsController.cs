using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Application.Command;
using Payment.Application.DTOs;
using Payment.Application.Queries;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllPaymentsQuery(), ct);

            return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResult( result,message: "Payments retrieved successfully",
               statusCode: 200));
        }


        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetPaymentByIdQuery(id), ct);

            if (result is null)return NotFound(ApiResponse<string>.FailResult(message: "Payment not found", statusCode: 404));

            return Ok(ApiResponse<PaymentDto>.SuccessResult( result,message: "Payment retrieved successfully",statusCode: 200));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<StripePaymentResponseDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreatePaymentCommand(dto), ct);

            return StatusCode(201, ApiResponse<StripePaymentResponseDto>.SuccessResult( result,message: 
                "Payment intent created successfully",statusCode: 201));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePaymentDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdatePaymentCommand(id, dto), ct);

            if (result is null)return NotFound(ApiResponse<string>.FailResult( message: "Payment not found",statusCode: 404));

            return Ok(ApiResponse<PaymentDto>.SuccessResult(result,message: "Payment updated successfully",statusCode: 200));
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _mediator.Send(new DeletePaymentCommand(id), ct);

            if (!deleted) return NotFound(ApiResponse<string>.FailResult(  message: "Payment not found",  statusCode: 404));

            return Ok(ApiResponse<string>.SuccessResult(data: null, message: "Payment deleted successfully",statusCode: 200));
        }
    }
}
