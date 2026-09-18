using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs.Category;
using ProductManager.Application.Interfaces;

namespace ProductManager.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            var result = await categoryService.GetAllAsync(includeDeleted, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await categoryService.GetByIdAsync(id, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
        {
            var category = await categoryService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken)
        {
            var category = await categoryService.UpdateAsync(id, dto, cancellationToken);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
        {
            bool success = await categoryService.SoftDeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id, CancellationToken cancellationToken)
        {
            bool success = await categoryService.RestoreAsync(id, cancellationToken);
            return success ? Ok() : NotFound();
        }
    }
}
