using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;
using ProductManager.Infrastructure.Persistence;

namespace ProductManager.Infrastructure.Repositories
{
    class AccountRepository(AppDbContext context) : IAccountRepository
    {
        public async Task<Account?> GetByEmailAsync(string email, CancellationToken cancellationToken = default) =>
            await context.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == email.ToLower(), cancellationToken);

        public async Task<Account?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            await context.Accounts.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            await context.Accounts.AddAsync(account, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
        {
            context.Accounts.Update(account);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
