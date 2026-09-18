using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Common
{
    public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalItems)
    {
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / (PageSize > 0 ? PageSize : 1));
    }
}
