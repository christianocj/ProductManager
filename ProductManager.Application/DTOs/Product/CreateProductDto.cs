using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Product
{
    public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    int CategoryId,
    string? ImageUrl = null);
}
