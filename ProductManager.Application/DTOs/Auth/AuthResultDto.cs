using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Auth
{
    public record AuthResultDto(
    bool Success,
    string? ErrorMessage = null,
    int? AccountId = null,
    string? Name = null,
    string? Email = null);
}
