using ProductManager.Application.DTOs.Category;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.Services
{
    public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
    {
        public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await categoryRepository.GetByIdAsync(id, includeDeleted: true, cancellationToken);
            return category is null ? null : MapToDto(category);
        }

        public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(bool includeDeleted = false, CancellationToken cancellationToken = default)
        {
            var categories = await categoryRepository.GetAllAsync(includeDeleted, cancellationToken);
            return categories.Select(MapToDto).ToList();
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("O nome da categoria é obrigatório.");

            if (await categoryRepository.ExistsByNameAsync(dto.Name, cancellationToken: cancellationToken))
                throw new InvalidOperationException("Já existe uma categoria cadastrada com este nome.");

            var category = new Category(dto.Name);
            await categoryRepository.AddAsync(category, cancellationToken);

            return MapToDto(category);
        }

        public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default)
        {
            var category = await categoryRepository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
            if (category is null)
                return null;

            if (await categoryRepository.ExistsByNameAsync(dto.Name, excludeId: id, cancellationToken: cancellationToken))
                throw new InvalidOperationException("Já existe outra categoria cadastrada com este nome.");

            category.Update(dto.Name);
            await categoryRepository.UpdateAsync(category, cancellationToken);

            return MapToDto(category);
        }

        public async Task<bool> SoftDeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await categoryRepository.GetByIdAsync(id, includeDeleted: false, cancellationToken);
            if (category is null)
                return false;

            category.Delete();
            await categoryRepository.UpdateAsync(category, cancellationToken);
            return true;
        }

        public async Task<bool> RestoreAsync(int id, CancellationToken cancellationToken = default)
        {
            var category = await categoryRepository.GetByIdAsync(id, includeDeleted: true, cancellationToken);
            if (category is null || !category.IsDeleted)
                return false;

            category.Restore();
            await categoryRepository.UpdateAsync(category, cancellationToken);
            return true;
        }

        private static CategoryDto MapToDto(Category category) =>
            new(category.Id, category.Name, category.IsDeleted, category.CreatedAt, category.UpdatedAt);
    }
}
