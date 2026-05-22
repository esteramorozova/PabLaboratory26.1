using AppCore.Dto;

namespace AppCore.Interfaces;

public interface IAdminService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(string userId);
    Task AssignRoleAsync(string userId, string role);
    Task RevokeRoleAsync(string userId, string role);
    Task LockUserAsync(string userId);
    Task UnlockUserAsync(string userId);
    Task DeactivateUserAsync(string userId);
    Task ActivateUserAsync(string userId);
    Task DeleteUserAsync(string userId);
}