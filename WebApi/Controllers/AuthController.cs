using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(UserManager<CrmUser> userManager) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized(new { message = "Nieprawidlowy email lub haslo." });
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!passwordValid)
        {
            return Unauthorized(new { message = "Nieprawidlowy email lub haslo." });
        }

        var roles = await userManager.GetRolesAsync(user);
        return Ok(new
        {
            message = "Logowanie poprawne.",
            userId = user.Id,
            email = user.Email,
            roles
        });
    }
}

public record LoginRequest(string Email, string Password);

