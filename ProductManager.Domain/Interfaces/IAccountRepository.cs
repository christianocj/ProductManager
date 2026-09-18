using ProductManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Account?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(Account account, CancellationToken cancellationToken = default);
        Task UpdateAsync(Account account, CancellationToken cancellationToken = default);
    }
}
