using AppCore.Interfaces;
using AppCore.Models;

namespace Infrastructure.Memory;

public class MemoryPersonRepository : MemoryGenericRepository<Person>, IPersonRepository
{
    public MemoryPersonRepository()
    {
        var companyId = Guid.NewGuid();
        var orgId = Guid.NewGuid();

        var company = new Company
        {
            Id = companyId,
            Name = "Example Sp. z o.o.",
            Email = "contact@example.com",
            Phone = "111-222-333",
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Street = "Company St 1",
                City = "Warsaw",
                PostalCode = "00-001",
                Country = "PL",
                Type = AddressType.Main
            },
            CreatedAt = DateTime.UtcNow,
            Status = ContactStatus.Active
        };

        var organization = new Organization
        {
            Id = orgId,
            Name = "Example Foundation",
            Type = OrganizationType.Foundation
        };

        var p1Id = Guid.NewGuid();
        _data.Add(p1Id, new Person
        {
            Id = p1Id,
            FirstName = "Adam",
            LastName = "Nowak",
            Gender = Gender.Male,
            Email = "adam.nowak@example.com",
            Phone = "123-456-789",
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Street = "Main St 1",
                City = "Krakow",
                PostalCode = "30-001",
                Country = "PL",
                Type = AddressType.Main
            },
            CreatedAt = DateTime.UtcNow,
            Status = ContactStatus.Active,
            Employer = company,
            Organization = organization
        });

        var p2Id = Guid.NewGuid();
        _data.Add(p2Id, new Person
        {
            Id = p2Id,
            FirstName = "Ewa",
            LastName = "Kowalska",
            Gender = Gender.Female,
            Email = "ewa.kowalska@example.com",
            Phone = "987-654-321",
            Address = new Address
            {
                Id = Guid.NewGuid(),
                Street = "Second St 2",
                City = "Gdansk",
                PostalCode = "80-001",
                Country = "PL",
                Type = AddressType.Correspondence
            },
            CreatedAt = DateTime.UtcNow,
            Status = ContactStatus.Active,
            Employer = company,
            Organization = organization
        });
    }

    public Task<IEnumerable<Person>> FindByEmployerAsync(Guid companyId)
    {
        IEnumerable<Person> result = _data.Values.Where(p => p.Employer != null && p.Employer.Id == companyId);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Person>> FindByOrganizationAsync(Guid organizationId)
    {
        IEnumerable<Person> result = _data.Values.Where(p => p.Organization != null && p.Organization.Id == organizationId);
        return Task.FromResult(result);
    }
    
    public Task AddNoteToPersonAsync(Note note)
    {
        return Task.CompletedTask;
    }
}

