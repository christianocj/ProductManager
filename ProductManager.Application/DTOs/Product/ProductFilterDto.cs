using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Product
{
    public record ProductFilterDto
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 12;
        public string? Search { get; init; }
        public int? CategoryId { get; init; }
    }
}
