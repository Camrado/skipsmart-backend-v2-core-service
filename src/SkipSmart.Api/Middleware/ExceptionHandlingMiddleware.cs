using Microsoft.AspNetCore.Mvc;
using SkipSmart.Application.Exceptions;

namespace SkipSmart.Api.Middleware;

public class ExceptionHandlingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context);
        } catch (Exception e) {
            _logger.LogError(e, "Exception occured: {Message}", e.Message);

            var exceptionDetails = GetExceptionDetails(e);

            var problemDetails = new ProblemDetails {
                Status = exceptionDetails.StatusCode,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail
            };

            if (exceptionDetails.Errors is not null) {
                problemDetails.Extensions["errors"] = exceptionDetails.Errors;
            }
            
            context.Response.StatusCode = exceptionDetails.StatusCode;
            
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
    
    private static ExceptionDetails GetExceptionDetails(Exception exception) {
        return exception switch {
            ValidationException validationException => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validation error",
                "One or more validation errors has occurred",
                validationException.Errors),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Server error",
                "An unexpected error has occurred",
                null)
        };
    }
    
    private record ExceptionDetails(
        int StatusCode,
        string Type,
        string Title,
        string Detail,
        IEnumerable<object>? Errors);
}