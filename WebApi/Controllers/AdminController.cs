using AppCore.Authorization;
using AppCore.Dto;
using AppCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Policy = nameof(CrmPolicies.AdminOnly))]
public class AdminController(IAdminService adminService) : ControllerBase
{
    /// <summary>Pobierz listę wszystkich użytkowników.</summary>
    [HttpGet("users")]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await adminService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>Pobierz użytkownika po ID.</summary>
    [HttpGet("users/{userId}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(string userId)
    {
        var user = await adminService.GetUserByIdAsync(userId);
        return Ok(user);
    }

    /// <summary>Przypisz rolę użytkownikowi.</summary>
    [HttpPost("users/{userId}/roles/{role}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        await adminService.AssignRoleAsync(userId, role);
        return NoContent();
    }

    /// <summary>Cofnij rolę użytkownikowi.</summary>
    [HttpDelete("users/{userId}/roles/{role}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RevokeRole(string userId, string role)
    {
        await adminService.RevokeRoleAsync(userId, role);
        return NoContent();
    }

    /// <summary>Zablokuj konto użytkownika.</summary>
    [HttpPost("users/{userId}/lock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LockUser(string userId)
    {
        await adminService.LockUserAsync(userId);
        return NoContent();
    }

    /// <summary>Odblokuj konto użytkownika.</summary>
    [HttpPost("users/{userId}/unlock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnlockUser(string userId)
    {
        await adminService.UnlockUserAsync(userId);
        return NoContent();
    }

    /// <summary>Dezaktywuj użytkownika.</summary>
    [HttpPost("users/{userId}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateUser(string userId)
    {
        await adminService.DeactivateUserAsync(userId);
        return NoContent();
    }

    /// <summary>Aktywuj użytkownika.</summary>
    [HttpPost("users/{userId}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateUser(string userId)
    {
        await adminService.ActivateUserAsync(userId);
        return NoContent();
    }

    /// <summary>Usuń użytkownika.</summary>
    [HttpDelete("users/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await adminService.DeleteUserAsync(userId);
        return NoContent();
    }
}