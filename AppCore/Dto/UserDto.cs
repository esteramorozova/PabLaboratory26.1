namespace AppCore.Dto;

public record UserDto
{
    public string Id { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string? Position { get; init; }
    public SystemUserStatus Status { get; init; }
    public bool IsLocked { get; init; }           
    public DateTimeOffset? LockoutEnd { get; init; }
    public IEnumerable<string> Roles { get; init; } = [];
    public DateTime CreatedAt { get; init; }
    public DateTime? LastLoginAt { get; init; }
}