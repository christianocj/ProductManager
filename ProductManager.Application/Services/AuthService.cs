using ProductManager.Application.DTOs.Auth;
using ProductManager.Application.Interfaces;
using ProductManager.Domain.Entities;
using ProductManager.Domain.Interfaces;

namespace ProductManager.Application.Services
{
    public class AuthService(
        IAccountRepository accountRepository,
        IPasswordHasher passwordHasher) : IAuthService
    {
        public async Task<AuthResultDto> ValidateCredentialsAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                return new AuthResultDto(false, "E-mail e senha são obrigatórios.");

            string normalizedEmail = loginDto.Email.Trim().ToLowerInvariant();
            var account = await accountRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

            if (account is null || !account.IsActive)
                return new AuthResultDto(false, "Credenciais inválidas.");

            bool isValidPassword = passwordHasher.VerifyPassword(loginDto.Password, account.PasswordHash);
            if (!isValidPassword)
                return new AuthResultDto(false, "Credenciais inválidas.");

            return new AuthResultDto(true, null, account.Id, account.Name, account.Email);
        }

        public async Task SeedInitialAccountAsync(string name, string email, string password, CancellationToken cancellationToken = default)
        {
            string normalizedEmail = email.Trim().ToLowerInvariant();
            var existingAccount = await accountRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

            if (existingAccount is null)
            {
                string passwordHash = passwordHasher.HashPassword(password);
                var account = new Account(name, normalizedEmail, passwordHash);
                await accountRepository.AddAsync(account, cancellationToken);
            }
        }
    }
}
