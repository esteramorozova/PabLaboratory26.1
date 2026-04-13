using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppCore.Models;
using AppCore;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.EntityFramework.Context;
public class ContactsDbContext: IdentityDbContext<CrmUser, CrmRole, string>
{
    public const string AdminUserId = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
    public const string SalesUserId = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";

    public const string RoleIdAdministrator = "10000000-0000-0000-0000-000000000001";
    public const string RoleIdSalesManager = "10000000-0000-0000-0000-000000000002";
    public const string RoleIdSalesperson = "10000000-0000-0000-0000-000000000003";
    public const string RoleIdSupportAgent = "10000000-0000-0000-0000-000000000004";
    public const string RoleIdReadOnly = "10000000-0000-0000-0000-000000000005";

    private const string PasswordHashUser1 =
        "AQAAAAIAAYagAAAAEC8DQ4eH7ICLtmAealgp456gDxD5xYkNalo0rIA+7fo5kSRwdJIbAB+0h5YYmYgLbA==";

    private const string PasswordHashUser2 =
        "AQAAAAIAAYagAAAAEC8DQ4eH7ICLtmAealgp456gDxD5xYkNalo0rIA+7fo5kSRwdJIbAB+0h5YYmYgLbA==";

    private static readonly DateTime SeedTimestamp = new(2024, 1, 15, 8, 0, 0, DateTimeKind.Unspecified);

    public DbSet<Person> People { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;

        var dataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PabLaboratory26");
        Directory.CreateDirectory(dataDir);
        var dbPath = Path.Combine(dataDir, "contacts.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    public ContactsDbContext()
    {
    }

    public ContactsDbContext(DbContextOptions<ContactsDbContext> options) :
        base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // wymagane przez Identity
        
        builder.Entity<CrmUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
        });
        
        builder.Entity<CrmRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(20);
            entity.Property(r => r.NormalizedName).HasMaxLength(20);
        });

        builder.Entity<CrmRole>().HasData(
            new CrmRole(UserRole.Administrator.ToString())
            {
                Id = RoleIdAdministrator,
                NormalizedName = UserRole.Administrator.ToString().ToUpperInvariant()
            },
            new CrmRole(UserRole.SalesManager.ToString())
            {
                Id = RoleIdSalesManager,
                NormalizedName = UserRole.SalesManager.ToString().ToUpperInvariant()
            },
            new CrmRole(UserRole.Salesperson.ToString())
            {
                Id = RoleIdSalesperson,
                NormalizedName = UserRole.Salesperson.ToString().ToUpperInvariant()
            },
            new CrmRole(UserRole.SupportAgent.ToString())
            {
                Id = RoleIdSupportAgent,
                NormalizedName = UserRole.SupportAgent.ToString().ToUpperInvariant()
            },
            new CrmRole(UserRole.ReadOnly.ToString())
            {
                Id = RoleIdReadOnly,
                NormalizedName = UserRole.ReadOnly.ToString().ToUpperInvariant()
            });

        builder.Entity<CrmUser>().HasData(
            new CrmUser
            {
                Id = AdminUserId,
                UserName = "admin@wsei.edu.pl",
                NormalizedUserName = "ADMIN@WSEI.EDU.PL",
                Email = "admin@wsei.edu.pl",
                NormalizedEmail = "ADMIN@WSEI.EDU.PL",
                EmailConfirmed = true,
                PasswordHash = PasswordHashUser1,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FirstName = "Jan",
                LastName = "Kowalski",
                FullName = "Jan Kowalski",
                Department = "Administracja",
                Status = SystemUserStatus.Active,
                CreatedAt = SeedTimestamp
            },
            new CrmUser
            {
                Id = SalesUserId,
                UserName = "anna.sales@wsei.edu.pl",
                NormalizedUserName = "ANNA.SALES@WSEI.EDU.PL",
                Email = "anna.sales@wsei.edu.pl",
                NormalizedEmail = "ANNA.SALES@WSEI.EDU.PL",
                EmailConfirmed = true,
                PasswordHash = PasswordHashUser2,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                FirstName = "Anna",
                LastName = "Nowak",
                FullName = "Anna Nowak",
                Department = "Sprzedaż",
                Status = SystemUserStatus.Active,
                CreatedAt = SeedTimestamp
            });

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = AdminUserId, RoleId = RoleIdAdministrator },
            new IdentityUserRole<string> { UserId = SalesUserId, RoleId = RoleIdSalesperson });
        
        // Konfiguracji mapowania dziedziczenia TPH
        // Jedna tabela do przechowywnia wszystkich typów kontaktów
        builder.Entity<Contact>()
            .HasDiscriminator<string>("ContactType")
            .HasValue<Person>("Person")
            .HasValue<Company>("Company");

        builder.Entity<Contact>(entity =>
        {
            entity.Property(p => p.Email).HasMaxLength(200);
            entity.Property(p => p.Phone).HasMaxLength(20);
            // dodoj ograniczenia dla pozostałych właściwości 
        });
        
        builder.Entity<Person>(entity =>
        {
            entity.Property(p => p.BirthDate).HasColumnType("date");
            entity.Property(p => p.Gender).HasConversion<string>();
            entity.Property(p => p.Status).HasConversion<string>();
            // dodaj ograniczenia dla pozostałych właściwości
        });
        
        // definicja związku
        builder.Entity<Person>()
            .HasOne(p => p.Employer)
            .WithMany(e => e.Employees);

        
        // definicja związku        
        builder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(p => p.Organization);
        
        // przykładowa firma
        builder.Entity<Company>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271"),
                    Name = "WSEI",
                    Industry = "edukacja",
                    Phone = "123567123",
                    Email = "biuro@wsei.edu.pl",
                    Website = "https://wsei.edu.pl",
                    Status = ContactStatus.Active,
                    CreatedAt = SeedTimestamp,
                    UpdatedAt = SeedTimestamp
                }
            );
        });
        
        var address = new
        {
            City = "Kraków",
            Country = "Poland",
            PostalCode = "25-009",
            Street = "ul. Św. Filipa 17",
            Type = AddressType.Correspondence,
            // id osoby, która dodana jest niżej
            ContactId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f")
        };
        
        // przykładowe kontakty typu Person
        builder.Entity<Person>(entity =>
        {
            entity.HasData(
                new
                {
                    Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f"),
                    FirstName = "Adam",
                    LastName = "Nowak",
                    Gender = Gender.Male,
                    Status = ContactStatus.Active,
                    Email = "adam@wsei.edu.pl",
                    Phone = "123456789",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Programista",
                    CreatedAt = SeedTimestamp,
                    UpdatedAt = SeedTimestamp
                },
                new 
                {
                    Id = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466"),
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    Gender = Gender.Female,
                    Status = ContactStatus.Blocked,
                    Email = "ewa@wsei.edu.pl",
                    Phone = "123123123",
                    BirthDate = DateTime.Parse("2001-01-11"),
                    Position = "Tester",
                    CreatedAt = SeedTimestamp,
                    UpdatedAt = SeedTimestamp
                });
        });
        //mapowanie adresu jako typu osadzonej w encji Contact 
        builder.Entity<Contact>()
            .OwnsOne(c => c.Address)
            .HasData(
                address,
                new
                {
                    City = "Kraków",
                    Country = "Poland",
                    PostalCode = "30-001",
                    Street = "ul. Grodzka 1",
                    Type = AddressType.Main,
                    ContactId = Guid.Parse("B4DCB17C-F875-43F8-9D66-36597895A466")
                },
                new
                {
                    City = "Lublin",
                    Country = "Poland",
                    PostalCode = "20-038",
                    Street = "ul. Projektowa 4",
                    Type = AddressType.Main,
                    ContactId = Guid.Parse("516A34D7-CCFB-4F20-85F3-62BD0F3AF271")
                });
    }
}