using ProductManager.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CategoryDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
        Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> RestoreAsync(int id, CancellationToken cancellationToken = default);
    }
}
