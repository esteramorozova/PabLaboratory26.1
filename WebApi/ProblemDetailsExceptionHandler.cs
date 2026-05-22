using AppCore.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace WebApi;

public class ProblemDetailsExceptionHandler(
    ProblemDetailsFactory factory,
    ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ContactNotFoundException)
        {
            logger.Log(LogLevel.Information, $"Exception '{exception.Message}' handled!");
            var problem = factory.CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest,
                "Contact service error!",
                "Service error",
                detail: exception.Message
            );
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }

        if (exception is UserNotFoundException) 
        {
            logger.Log(LogLevel.Information, $"Exception '{exception.Message}' handled!");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message }, cancellationToken);
            return true;
        }

        if (exception.Message.Contains("Nieprawidłowy email") || 
            exception.Message.Contains("Konto jest") ||
            exception.Message.Contains("Nieprawidłowy token") ||
            exception.Message.Contains("Nieprawidłowy refresh"))
        {
            logger.Log(LogLevel.Information, $"Auth exception '{exception.Message}' handled!");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message }, cancellationToken);
            return true;
        }
        
        return false;
    }
}