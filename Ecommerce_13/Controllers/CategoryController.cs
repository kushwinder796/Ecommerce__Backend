using Catalog.Application.Command;
using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Ecommerce_13.Comman;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_13.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(ApiResponse<List<CategoryDto>>.SuccessResult(result,"Categories retrieved successfully"));
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));

            if (result == null)
                throw new KeyNotFoundException($"Category with id {id} not found");

            return Ok(ApiResponse<CategoryDto>.SuccessResult(result,"Category retrieved successfully"));
        }


        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateCategoryCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id },
                ApiResponse<int>.SuccessResult(id, "Category created successfully", 201));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
                throw new ArgumentException("ID mismatch");

            var result = await _mediator.Send(command);

            if (!result)
                throw new KeyNotFoundException($"Category with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Updated", "Category updated successfully"));
        }

      
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));

            if (!result)throw new KeyNotFoundException($"Category with id {id} not found");

            return Ok(ApiResponse<string>.SuccessResult("Deleted", "Category deleted successfully"));
        }

    }
}
