using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ManagementService.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found."),
            ArgumentException argEx => (StatusCodes.Status400BadRequest, 
                string.IsNullOrWhiteSpace(argEx.Message) ? "Bad request." : argEx.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized access."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        // Log unexpected exceptions
        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new
        {
            statusCode,
            message
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
