using AppCore;
using AppCore.Dto;
using AppCore.Exceptions;
using AppCore.Interfaces;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Security;

public class AdminService : IAdminService
{
    private readonly UserManager<CrmUser> _userManager;

    public AdminService(UserManager<CrmUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(MapToDto(user, roles));
        }

        return result;
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        var roles = await _userManager.GetRolesAsync(user);
        return MapToDto(user, roles);
    }

    public async Task AssignRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        if (!await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                throw new Exception($"Błąd przypisania roli: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    public async Task RevokeRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        if (await _userManager.IsInRoleAsync(user, role))
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
                throw new Exception($"Błąd cofania roli: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    public async Task LockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
    }

    public async Task UnlockUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        await _userManager.SetLockoutEndDateAsync(user, null);
        await _userManager.ResetAccessFailedCountAsync(user);
    }

    public async Task DeactivateUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        user.Deactivate(DateTime.UtcNow);
        await _userManager.UpdateAsync(user);
    }

    public async Task ActivateUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        user.Activate();
        await _userManager.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new UserNotFoundException($"Użytkownik o id={userId} nie istnieje.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new Exception($"Błąd usuwania użytkownika: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    private static UserDto MapToDto(CrmUser user, IList<string> roles) => new()
    {
        Id = user.Id,
        Email = user.Email ?? string.Empty,
        FirstName = user.FirstName,
        LastName = user.LastName,
        FullName = user.FullName,
        Department = user.Department,
        Status = user.Status,
        IsLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow,
        LockoutEnd = user.LockoutEnd,
        Roles = roles,
        CreatedAt = user.CreatedAt,
        LastLoginAt = user.LastLoginAt
    };
}