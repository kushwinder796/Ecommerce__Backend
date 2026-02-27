using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Offers.Application.Command;
using Offers.Application.DTOs;
using Offers.Application.Queries;
using Order.Application.Queries;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OffersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<OfferDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetAllOffersQuery(), ct);

            return Ok(ApiResponse<IEnumerable<OfferDto>>.SuccessResult(result, message: "Offers retrieved successfully",
               statusCode: 200));
         }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<OfferDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetOfferByIdQuery(id), ct);

            if (result is null)
                return NotFound(ApiResponse<string>.FailResult( message: "Offer not found",statusCode: 404));

            return Ok(ApiResponse<OfferDto>.SuccessResult( result,message: "Offer retrieved successfully",statusCode: 200));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OfferDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOfferDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new CreateOfferCommand(dto), ct);

            return CreatedAtAction(nameof(GetById), new { id = result.Id },
                ApiResponse<OfferDto>.SuccessResult(result, message: "Offer created successfully", statusCode: 201));
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<OfferDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOfferDto dto, CancellationToken ct)
        {
            var result = await _mediator.Send(new UpdateOfferCommand(id, dto), ct);

            if (result is null) return NotFound(ApiResponse<string>.FailResult(message: "Offer not found",statusCode: 404));

            return Ok(ApiResponse<OfferDto>.SuccessResult(result,message: "Offer updated successfully",statusCode: 200));
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _mediator.Send(new DeleteOfferCommand(id), ct);

            if (!deleted)
                return NotFound(ApiResponse<string>.FailResult( message: "Offer not found",statusCode: 404));

            return Ok(ApiResponse<string>.SuccessResult(data: null, message: "Offer deleted successfully",statusCode: 200));
        }
    }
}
