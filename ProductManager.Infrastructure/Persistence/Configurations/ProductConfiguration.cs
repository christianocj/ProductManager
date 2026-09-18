using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManager.Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Infrastructure.Persistence.Configurations
{  
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(500);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Global Query Filter para garantir que produtos eliminados não apareçam por padrão
            builder.HasQueryFilter(p => !p.IsDeleted);

            // Índice GIN no PostgreSQL para otimizar Full-Text Search em Name e Description.
            // O PostgreSQL utiliza tsvector para analisar lexemas das colunas configuradas.
            builder.HasIndex(p => new { p.Name, p.Description })
                .HasMethod("GIN");
        }
    }
}
