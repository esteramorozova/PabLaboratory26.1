using AppCore.Interfaces;
using AppCore.Models;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfPersonRepository(ContactsDbContext context)
    : EfGenericRepository<Person>(context.People), IPersonRepository
{
    public override async Task<Person?> FindByIdAsync(Guid id)
    {
        return await context.People
            .Include(p => p.Notes)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddNoteToPersonAsync(Note note)
    {
        await context.Set<Note>().AddAsync(note);
    }

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