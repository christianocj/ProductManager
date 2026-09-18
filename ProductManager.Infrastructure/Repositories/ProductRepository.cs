using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Infrastructure.Repositories
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task<Product?> GetByIdAsync(int id, bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Product> query = context.Products.Include(p => p.Category);
            if (includeDeleted)
                query = query.IgnoreQueryFilters();

            return await query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<Product> Items, int TotalCount)> GetPagedAsync(
            int page,
            int pageSize,
            string? search,
            int? categoryId,
            bool onlyDeleted = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Product> query = context.Products.Include(p => p.Category);

            // Tratamento de filtros globais de exclusão lógica
            if (onlyDeleted)
            {
                query = query.IgnoreQueryFilters().Where(p => p.IsDeleted);
            }

            // Filtro relacional por Categoria
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            // PostgreSQL Full-Text Search
            // Converte Name e Description para tsvector no motor de busca do PostgreSQL
            // O formato de busca utiliza operadores lexicais e sufixo ':*' para suporte a prefixos parciais.
            if (!string.IsNullOrWhiteSpace(search))
            {
                string cleanSearch = search.Trim();
                var terms = cleanSearch
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => $"{t}:*");

                string formattedQuery = string.Join(" & ", terms);

                query = query.Where(p =>
                    EF.Functions.ToTsVector("portuguese", p.Name + " " + p.Description)
                        .Matches(EF.Functions.ToTsQuery("portuguese", formattedQuery)));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await context.Products.AddAsync(product, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default) =>
            await context.Products.AnyAsync(p => p.Id == id, cancellationToken);

        public async Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default) =>
            await context.Products.CountAsync(cancellationToken);

        public async Task<int> GetDeletedCountAsync(CancellationToken cancellationToken = default) =>
            await context.Products.IgnoreQueryFilters().CountAsync(p => p.IsDeleted, cancellationToken);
    }
}
