using ProductManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id, bool includeDeleted = false, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            int? categoryId,
            bool onlyDeleted = false,
            CancellationToken cancellationToken = default);
        Task AddAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default);
        Task<int> GetDeletedCountAsync(CancellationToken cancellationToken = default);
    }
}
