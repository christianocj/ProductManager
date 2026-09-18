using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Interfaces;

namespace ProductManager.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/products")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] ProductFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await productService.GetPagedAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("deleted")]
        public async Task<IActionResult> GetDeletedPaged([FromQuery] ProductFilterDto filter, CancellationToken cancellationToken)
        {
            var result = await productService.GetDeletedPagedAsync(filter, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            var product = await productService.GetByIdAsync(id, includeDeleted, cancellationToken);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto, CancellationToken cancellationToken)
        {
            var product = await productService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto, CancellationToken cancellationToken)
        {
            var product = await productService.UpdateAsync(id, dto, cancellationToken);
            return product is null ? NotFound() : Ok(product);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
        {
            bool success = await productService.SoftDeleteAsync(id, cancellationToken);
            return success ? NoContent() : NotFound();
        }

        [HttpPost("{id:int}/restore")]
        public async Task<IActionResult> Restore(int id, CancellationToken cancellationToken)
        {
            bool success = await productService.RestoreAsync(id, cancellationToken);
            return success ? Ok() : NotFound();
        }
    }
}