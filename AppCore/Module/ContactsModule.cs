using FluentValidation;
using AppCore.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Rejestracja walidatorów
        services.AddValidatorsFromAssemblyContaining<CreatePersonDtoValidator>();
        return services;
    }
}