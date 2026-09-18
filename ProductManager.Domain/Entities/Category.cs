using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Domain.Entities
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public ICollection<Product> Products { get; private set; } = new List<Product>();

        // Construtor privado para EF Core
        private Category() { }

        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome da categoria é obrigatório.", nameof(name));

            Name = name.Trim();
            CreatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public void Update(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome da categoria é obrigatório.", nameof(name));

            Name = name.Trim();
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
    }
}
