using Serilog.Context;

namespace SkipSmart.Api.Middleware;

public class RequestContextLoggingMiddleware {
    private const string CorrelationIdHeader = "X-Correlation-Id";
    
    private readonly RequestDelegate _next;
    
    public RequestContextLoggingMiddleware(RequestDelegate next) {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext) {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext))) {
            return _next(httpContext);
        }
    }
    
    private static string GetCorrelationId(HttpContext httpContext) {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}