using Microsoft.Extensions.Logging;

namespace SkipSmart.Api.Middleware;

public class RequestContextLoggingMiddleware {
    private const string CorrelationIdHeader = "X-Correlation-Id";
    
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextLoggingMiddleware> _logger;
    
    public RequestContextLoggingMiddleware(RequestDelegate next, ILogger<RequestContextLoggingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext) {
        var correlationId = GetCorrelationId(httpContext);

        using (_logger.BeginScope(new Dictionary<string, object> {
            ["CorrelationId"] = correlationId
        })) {
            await _next(httpContext);
        }
    }
    
    private static string GetCorrelationId(HttpContext httpContext) {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}