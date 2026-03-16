using AppCore.Models;

namespace AppCore.Interfaces;

public interface IPersonRepositoryAsync : IGenericRepositoryAsync<Person>
{
    Task<IEnumerable<Person>> FindByEmployerAsync(Guid companyId);
    Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId);
}

