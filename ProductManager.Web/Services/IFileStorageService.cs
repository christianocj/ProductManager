namespace ProductManager.Web.Services;

using Microsoft.AspNetCore.Http;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, CancellationToken cancellationToken = default);
    void DeleteFile(string? relativePath);
}