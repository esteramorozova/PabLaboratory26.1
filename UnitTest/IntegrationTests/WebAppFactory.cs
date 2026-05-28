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

            services.AddDbContext<ContactsDbContext>(options =>
            {
                options.UseInMemoryDatabase("CRM_Integration_Test_Database_Stable");
            });

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

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ContactsDbContext>();
        
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

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
                    
                }
            }
        }

        return host;
    }
}
