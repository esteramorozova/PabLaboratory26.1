using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfPersonRepository(ContactsDbContext context)
    : EfGenericRepository<Person>(context.People), IPersonRepository
{
    public async Task<IEnumerable<Person>> FindByEmployerAsync(Guid companyId)
    {
        return await context.People
            .AsNoTracking()
            .Where(p => EF.Property<Guid?>(p, "EmployerId") == companyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        return await context.People
            .AsNoTracking()
            .Where(p => EF.Property<Guid?>(p, "OrganizationId") == organizationId)
            .ToListAsync();
    }
}
