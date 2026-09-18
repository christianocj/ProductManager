using ProductManager.Application.DTOs.Common;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.Services
{
    public class ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository) : IProductService
    {
        public async Task<ProductDto?> GetByIdAsync(int id, bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            var product = await productRepository.GetByIdAsync(id, includeDeleted, cancellationToken);
            return product is null ? null : MapToDto(product);
        }

        public async Task<PagedResult<ProductDto>> GetPagedAsync(ProductFilterDto filter, CancellationToken cancellationToken = default)
        {
            int page = filter.Page < 1 ? 1 : filter.Page;
            int pageSize = filter.PageSize is < 1 or > 100 ? 12 : filter.PageSize;

            var (items, totalCount) = await productRepository.GetPagedAsync(
                page,
                pageSize,
                filter.Search,
                filter.CategoryId,
                onlyDeleted: false,
                cancellationToken);

            var dtos = items.Select(MapToDto).ToList();
            return new PagedResult<ProductDto>(dtos, page, pageSize, totalCount);
        }

        public async Task<PagedResult<ProductDto>> GetDeletedPagedAsync(ProductFilterDto filter, CancellationToken cancellationToken = default)
        {
            int page = filter.Page < 1 ? 1 : filter.Page;
            int pageSize = filter.PageSize is < 1 or > 100 ? 12 : filter.PageSize;

            var (items, totalCount) = await productRepository.GetPagedAsync(
                page,
                pageSize,
                filter.Search,
                filter.CategoryId,
                onlyDeleted: true,
                cancellationToken);

            var dtos = items.Select(MapToDto).ToList();
            return new PagedResult<ProductDto>(dtos, page, pageSize, totalCount);
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
        {
            if (!await categoryRepository.ExistsAsync(dto.CategoryId, cancellationToken))
                throw new InvalidOperationException("A categoria informada não existe ou foi removida.");

            var product = new Product(dto.Name, dto.Description, dto.Price, dto.CategoryId, dto.ImageUrl);
            await productRepository.AddAsync(product, cancellationToken);

            var reloadedProduct = await productRepository.GetByIdAsync(product.Id, includeDeleted: false, cancellationToken);
            return MapToDto(reloadedProduct ?? product);
        }

        public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
        {
            var product = await productRepository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
            if (product is null)
                return null;

            if (!await categoryRepository.ExistsAsync(dto.CategoryId, cancellationToken))
                throw new InvalidOperationException("A categoria informada não existe ou foi removida.");

            product.Update(dto.Name, dto.Description, dto.Price, dto.CategoryId, dto.ImageUrl);
            await productRepository.UpdateAsync(product, cancellationToken);

            var reloadedProduct = await productRepository.GetByIdAsync(product.Id, includeDeleted: false, cancellationToken);
            return MapToDto(reloadedProduct ?? product);
        }

        public async Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await productRepository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
            if (product is null)
                return false;

            product.Delete();
            await productRepository.UpdateAsync(product, cancellationToken);
            return true;
        }

        public async Task<bool> RestoreAsync(int id, CancellationToken cancellationToken = default)
        {
            var product = await productRepository.GetByIdAsync(id, includeDeleted: true, cancellationToken);
            if (product is null || !product.IsDeleted)
                return false;

            product.Restore();
            await productRepository.UpdateAsync(product, cancellationToken);
            return true;
        }

        public async Task<(int ActiveProducts, int Categories, int DeletedProducts)> GetDashboardMetricsAsync(CancellationToken cancellationToken = default)
        {
            int activeProducts = await productRepository.GetActiveCountAsync(cancellationToken);
            int categories = await categoryRepository.GetCountAsync(cancellationToken);
            int deletedProducts = await productRepository.GetDeletedCountAsync(cancellationToken);

            return (activeProducts, categories, deletedProducts);
        }

        private static ProductDto MapToDto(Product product) =>
            new(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.CategoryId,
                product.Category?.Name ?? string.Empty,
                product.ImageUrl,
                product.IsDeleted,
                product.CreatedAt,
                product.UpdatedAt);
    }
}
