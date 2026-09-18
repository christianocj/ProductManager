using ProductManager.Application.DTOs.Common;
using ProductManager.Application.DTOs.Product;
using System;
using System.Text;

namespace ProductManager.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id, bool includeDeleted = false, CancellationToken cancellationToken = default);
        Task<PagedResult<ProductDto>> GetPagedAsync(ProductFilterDto filter, CancellationToken cancellationToken = default);
        Task<PagedResult<ProductDto>> GetDeletedPagedAsync(ProductFilterDto filter, CancellationToken cancellationToken = default);
        Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default);
        Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> RestoreAsync(int id, CancellationToken cancellationToken = default);
        Task<(int ActiveProducts, int Categories, int DeletedProducts)> GetDashboardMetricsAsync(CancellationToken cancellationToken = default);
    }
}
