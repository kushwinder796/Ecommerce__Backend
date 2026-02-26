using Catalog.Application.Command;
using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.OpenApi.Any;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(ApiResponse<List<ProductDto>>.SuccessResult(result, "Products retrieved successfully "));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByid(Guid id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
             if (result == null)
              {
                throw new KeyNotFoundException($"Product with id {id} not found");
              }
            return Ok(ApiResponse<ProductDto>.SuccessResult(result, "Product retrieved successfully"));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]  
        public async Task<IActionResult> Create([FromForm] CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetByid),new { id },ApiResponse<Guid>.SuccessResult(
            id, "Product created successfully", 201));
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
                throw new ArgumentException("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result)
                throw new KeyNotFoundException($"Product with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult(
                "Updated", "Product updated successfully"));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (!result)
                throw new KeyNotFoundException($"Product with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult(
                "Deleted", "Product deleted successfully"));
        }

    }
}
