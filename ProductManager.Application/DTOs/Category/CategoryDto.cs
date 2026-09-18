using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Category
{
    public record CategoryDto(
    int Id,
    string Name,
    bool IsDeleted,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
}
