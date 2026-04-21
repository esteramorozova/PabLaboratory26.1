using AppCore.Interfaces;
using AppCore.Module;
using AppCore.Seeders;
using Infrastructure.Memory;
using Infrastructure.Security;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        /* //generator haszy Admin123!
        Console.Write("Podaj hasło: ");
        var password = Console.ReadLine() ?? "";
        var hasher = new PasswordHasher<IdentityUser>();
        var hash = hasher.HashPassword(new IdentityUser { UserName = "seed" }, password);
        Console.WriteLine("HASH:");
        Console.WriteLine(hash);
        */
        
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddContactsEfModule(builder.Configuration);
        builder.Services.AddContactsCoreModule(builder.Configuration);
        builder.Services.AddSingleton<JwtSettings>();
        builder.Services.AddJwt(new JwtSettings(builder.Configuration));
        builder.Services.AddSingleton<ICustomerService, MemoryCustomerService>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            using var scope = app.Services.CreateScope();
            var seeders = scope.ServiceProvider
                .GetServices<IDataSeeder>()
                .OrderBy(s => s.Order);

            foreach (var seeder in seeders)
                await seeder.SeedAsync();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionHandler();
        
        app.MapControllers();

        app.MapGet("/api/customers", (ICustomerService service,HttpContext httpContext) =>
            {
                return service.GetCustomers();
            })
            .WithName("GetCustomers")
            .AllowAnonymous();

        app.Run();
    }
    
}