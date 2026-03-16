using AppCore.Models;

namespace AppCore.Interfaces;

public interface ICompanyRepositoryAsync : IGenericRepositoryAsync<Company>
{
    Task<IEnumerable<Company>> FindByNameAsync(string namePart);
    Task<Company?> FindByNipAsync(string nip);
    Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId);
}

