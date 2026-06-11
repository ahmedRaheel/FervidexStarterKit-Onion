using Serilog.Context;

namespace StarterKit.Api.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        // Add to response headers for the client to track
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        // Push to Serilog's LogContext for automatic inclusion in all logs
        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
        {
            await _next(context);
        }
    }
}
