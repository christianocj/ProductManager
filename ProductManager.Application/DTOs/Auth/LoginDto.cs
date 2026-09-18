using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManager.Application.DTOs.Auth
{
    public record LoginDto(string Email, string Password);
}
