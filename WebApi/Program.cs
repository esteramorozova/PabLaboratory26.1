using AppCore.Interfaces;
using AppCore.Module;
using Infrastructure.Memory;
using Infrastructure.Security;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
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

        builder.Services.AddAuthorization();
        builder.Services.AddContactsEfModule(builder.Configuration);
        builder.Services.AddContactsCoreModule(builder.Configuration);
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