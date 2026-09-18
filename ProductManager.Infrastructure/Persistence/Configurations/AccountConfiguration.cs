using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManager.Domain.Entities;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Infrastructure.Persistence.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.HasIndex(a => a.Email)
                .IsUnique();

            builder.Property(a => a.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();
        }
    }
}
