using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryCompanyRepository : MemoryGenericRepository<Company>, ICompanyRepository
{
    public Task<IEnumerable<Company>> FindByNameAsync(string namePart)
    {
        throw new NotImplementedException();
    }

    public Task<Company?> FindByNipAsync(string nip)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetEmployeesAsync(Guid companyId)
    {
        throw new NotImplementedException();
    }
}

