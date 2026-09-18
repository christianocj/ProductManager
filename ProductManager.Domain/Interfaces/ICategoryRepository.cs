using ProductManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id, bool includeDeleted = false, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default);
        Task AddAsync(Category category, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    }
}
