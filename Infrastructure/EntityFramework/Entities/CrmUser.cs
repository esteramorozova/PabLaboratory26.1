using Microsoft.AspNetCore.Identity;
using AppCore;

namespace Infrastructure.EntityFramework.Entities;

public class CrmUser : IdentityUser, ISystemUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string FullName { get; set; }
    public required string Department { get; set; }
    public required SystemUserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }

    string ISystemUser.Id => Id;
    string ISystemUser.Email => Email ?? string.Empty;
    string ISystemUser.FirstName => FirstName;
    string ISystemUser.LastName => LastName;
    string ISystemUser.FullName => FullName;
    string ISystemUser.Department => Department;
    SystemUserStatus ISystemUser.Status => Status;
    DateTime ISystemUser.CreatedAt => CreatedAt;

    public void Activate()
    {
        if (Status == SystemUserStatus.Inactive)
        {
            Status = SystemUserStatus.Active;
        }
    }

    public void Deactivate(DateTime now)
    {
        if (Status == SystemUserStatus.Active)
        {
            Status = SystemUserStatus.Inactive;
            DeactivatedAt = now;
        }
    }
}