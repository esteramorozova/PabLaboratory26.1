using AppCore.Models;

namespace AppCore.Interfaces;

public interface IOrganizationRepositoryAsync : IGenericRepositoryAsync<Organization>
{
    Task<IEnumerable<Organization>> FindByTypeAsync(OrganizationType type);
    Task<IEnumerable<Person>> GetMembersAsync(Guid organizationId);
}

