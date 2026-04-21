using AppCore.Models;
using AppCore.Seeders;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeders;

public class ContactsDbSeeder : IDataSeeder
{
    public int Order => 2;

    private readonly ContactsDbContext _context;
    private readonly ILogger<ContactsDbSeeder> _logger;

    public ContactsDbSeeder(ContactsDbContext context, ILogger<ContactsDbSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.People.AnyAsync())
        {
            _logger.LogInformation("Kontakty już istnieją — pomijam.");
            return;
        }

        var people = new[]
        {
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Marek",
                LastName = "Wiśniewski",
                Email = "marek.wisniewski@example.com",
                Phone = "600-100-200",
                Gender = Gender.Male,
                Status = ContactStatus.Active,
                Position = "Kierownik",
                BirthDate = DateTime.Parse("1985-03-15"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "ul. Kwiatowa 5",
                    City = "Warszawa",
                    PostalCode = "00-100",
                    Country = "Poland",
                    Type = AddressType.Main
                }
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Katarzyna",
                LastName = "Zielińska",
                Email = "katarzyna.zielinska@example.com",
                Phone = "700-200-300",
                Gender = Gender.Female,
                Status = ContactStatus.Active,
                Position = "Analityk",
                BirthDate = DateTime.Parse("1990-07-22"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "ul. Słoneczna 12",
                    City = "Kraków",
                    PostalCode = "30-200",
                    Country = "Poland",
                    Type = AddressType.Main
                }
            },
            new Person
            {
                Id = Guid.NewGuid(),
                FirstName = "Tomasz",
                LastName = "Lewandowski",
                Email = "tomasz.lewandowski@example.com",
                Phone = "500-300-400",
                Gender = Gender.Male,
                Status = ContactStatus.Active,
                Position = "Programista",
                BirthDate = DateTime.Parse("1992-11-05"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Address = new Address
                {
                    Id = Guid.NewGuid(),
                    Street = "ul. Leśna 3",
                    City = "Wrocław",
                    PostalCode = "50-100",
                    Country = "Poland",
                    Type = AddressType.Main
                }
            }
        };

        await _context.People.AddRangeAsync(people);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Dodano {Count} kontaktów.", people.Length);
    }
}