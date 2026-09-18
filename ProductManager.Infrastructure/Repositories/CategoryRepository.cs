using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Persistence;
using System;
using System.Collections.Generic;

namespace ProductManager.Infrastructure.Repositories
{
    public class CategoryRepository(AppDbContext context) : ICategoryRepository
    {
        public async Task<Category?> GetByIdAsync(int id, bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> query = context.Categories;
            if (includeDeleted)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> query = context.Categories;
            if (includeDeleted)
                query = query.IgnoreQueryFilters();

            return await query.OrderBy(c => c.Name).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await context.Categories.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
        {
            context.Categories.Update(category);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
            await context.Categories.AnyAsync(c => c.Id == id, cancellationToken);

        public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
        {
            string normalized = name.Trim().ToLower();
            IQueryable<Category> query = context.Categories.Where(c => c.Name.ToLower() == normalized);

            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync(CancellationToken cancellationToken = default) =>
            await context.Categories.CountAsync(cancellationToken);
    }
}
