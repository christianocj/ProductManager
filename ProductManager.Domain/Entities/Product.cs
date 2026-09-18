using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int CategoryId { get; private set; }
        public string? ImageUrl { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Category Category { get; private set; } = null!;

        // Construtor privado para EF Core
        private Product() { }

        public Product(string name, string description, decimal price, int categoryId, string? imageUrl = null)
        {
            ValidateAndAssign(name, description, price, categoryId);

            ImageUrl = imageUrl;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string description, decimal price, int categoryId, string? imageUrl)
        {
            ValidateAndAssign(name, description, price, categoryId);

            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            IsDeleted = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Restore()
        {
            IsDeleted = false;
            UpdatedAt = DateTime.UtcNow;
        }

        private void ValidateAndAssign(string name, string description, decimal price, int categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome do produto é obrigatório.", nameof(name));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("A descrição do produto é obrigatória.", nameof(description));

            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price), "O preço deve ser maior que zero.");

            if (categoryId <= 0)
                throw new ArgumentException("Categoria inválida.", nameof(categoryId));

            Name = name.Trim();
            Description = description.Trim();
            Price = price;
            CategoryId = categoryId;
        }
    }
}
