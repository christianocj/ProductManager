using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Product
{
    public record ProductDto(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int CategoryId,
    string CategoryName,
    string? ImageUrl,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
}
