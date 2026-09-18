using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Domain.Entities
{

    public class Account
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Account() { }

        public Account(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome é obrigatório.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail é obrigatório.", nameof(email));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("O hash de senha é obrigatório.", nameof(passwordHash));

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            PasswordHash = passwordHash;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome é obrigatório.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail é obrigatório.", nameof(email));

            Name = name.Trim();
            Email = email.Trim().ToLowerInvariant();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("O hash de senha é obrigatório.", nameof(passwordHash));

            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
