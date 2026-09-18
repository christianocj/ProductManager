
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs.Auth;
using ProductManager.Application.Interfaces;

namespace ProductManager.Web.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
        {
            var result = await authService.ValidateCredentialsAsync(dto, cancellationToken);
            if (!result.Success)
                return BadRequest(new { message = result.ErrorMessage });

            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.AccountId!.Value.ToString()),
            new(ClaimTypes.Name, result.Name ?? string.Empty),
            new(ClaimTypes.Email, result.Email ?? string.Empty)
        };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new { message = "Login efetuado com sucesso." });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout efetuado com sucesso." });
        }
    }
}