using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using AppCore.Seeders;

namespace UnitTest.IntegrationTests;

public class WebAppFactory : WebApplicationFactory<WebApi.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // 1. usuwanie WSZYSTKICH rejestracji powiązanych z bazą danych i EF Core,
            // co uniemożliwi konflikt dostawców Sqlite oraz InMemory.
            var databaseDescriptors = services
                .Where(d => d.ServiceType.FullName != null &&
                            (d.ServiceType.FullName.Contains("DbContext") ||
                             d.ServiceType.FullName.Contains("EntityFrameworkCore") ||
                             d.ServiceType.FullName.Contains("DbConnection") ||
                             d.ServiceType.FullName.Contains("IDbContextOptionsConfiguration")))
                .ToList();

            foreach (var descriptor in databaseDescriptors)
            {
                services.Remove(descriptor);
            }

            // 2. Rejestracja czystego kontekstu bazy InMemory ze stabilną nazwą
            services.AddDbContext<ContactsDbContext>(options =>
            {
                options.UseInMemoryDatabase("CRM_Integration_Test_Database_Stable");
            });

            // 3. Konfiguracja Identity dostosowana do testów integracyjnych
            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
        });
    }

    // Wykonuje się bezpośrednio po zbudowaniu kontenera i postawieniu aplikacji serwerowej
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        // Tworzymy zakres do przygotowania schematu i danych testowych
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
            
            // Czyszczenie i przygotowanie tabel
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Pobranie i uruchomienie seeder'a tożsamości (IdentityDbSeeder)
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            var identitySeeder = seeders.FirstOrDefault(s => s.GetType().Name == "IdentityDbSeeder");

            if (identitySeeder != null)
            {
                identitySeeder.SeedAsync().GetAwaiter().GetResult();
            }
            else
            {
                try
                {
                    var seederType = Type.GetType("Infrastructure.Seeders.IdentityDbSeeder, Infrastructure");
                    if (seederType != null)
                    {
                        var manualSeeder = ActivatorUtilities.CreateInstance(scope.ServiceProvider, seederType) as IDataSeeder;
                        manualSeeder?.SeedAsync().GetAwaiter().GetResult();
                    }
                }
                catch
                {
                    // Wyciszenie ewentualnych błędów ładowania assembly podczas inicjalizacji środowiska testowego
                }
            }
        }

        return host;
    }
}