using ProductManager.Application.DTOs.Auth;
using System;

namespace ProductManager.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> ValidateCredentialsAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
        Task SeedInitialAccountAsync(string name, string email, string password, CancellationToken cancellationToken = default);
    }
}
