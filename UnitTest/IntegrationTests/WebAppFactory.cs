using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.EntityFramework.Context;

namespace UnitTest.IntegrationTests;

public class WebAppFactory : WebApplicationFactory<WebApi.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Usuń prawdziwą bazę SQLite
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ContactsDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Dodaj bazę InMemory do testów
            services.AddDbContext<ContactsDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        });
    }
}