using AppCore.Interfaces;
using AppCore.Module;
using Infrastructure.Memory;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddAuthorization();
        builder.Services.AddContactsModule(builder.Configuration); // moduł Contacts z walidatorami
        builder.Services.AddControllers();
        builder.Services.AddSingleton<ICustomerService, MemoryCustomerService>();
        
        builder.Services.AddSingleton<IPersonRepository, MemoryPersonRepository>();
        builder.Services.AddSingleton<ICompanyRepository, MemoryCompanyRepository>();
        builder.Services.AddSingleton<IOrganizationRepository, MemoryOrganizationRepository>();

        builder.Services.AddSingleton<IContactUnitOfWork, MemoryContactUnitOfWork>();
        builder.Services.AddSingleton<IPersonService, MemoryPersonService>();
        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();
        
        builder.Services.AddOpenApi();

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseExceptionHandler();
        
        app.MapControllers();

        app.MapGet("/api/customers", (ICustomerService service,HttpContext httpContext) =>
            {
                return service.GetCustomers();
            })
            .WithName("GetCustomers");

        app.Run();
    }
}