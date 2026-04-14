using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCompanyRepository(ContactsDbContext context)
    : EfGenericRepository<Company>(context.Companies), ICompanyRepository
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
