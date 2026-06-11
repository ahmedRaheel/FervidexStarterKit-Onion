using MediatR;
using Microsoft.Extensions.Logging;
namespace StarterKit.UseCase.Abstractions.Behaviors;
public sealed class LoggingBehavior<TRequest,TResponse>(ILogger<LoggingBehavior<TRequest,TResponse>> logger) : IPipelineBehavior<TRequest,TResponse> 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        logger.LogInformation("Processing {RequestName}", typeof(TRequest).Name);

        var response = await next();

        // Check if the response is of type Result<T> using reflection or pattern matching
        if (response is { } res && IsFailure(res))
        {
            logger.LogError("Request {RequestName} failed: {Error}",
                typeof(TRequest).Name, GetErrorMessage(res));
        }
        else
        {
            logger.LogInformation("Processed {RequestName} successfully", typeof(TRequest).Name);
        }

        return response;
    }

    // Helper to extract failure status from your generic Result<T>
    private static bool IsFailure(object response)
    {
        var property = response.GetType().GetProperty("IsFailure");
        return property != null && (bool)property.GetValue(response)!;
    }

    // Helper to extract the error message
    private static string GetErrorMessage(object response)
    {
        var errorProp = response.GetType().GetProperty("Error");
        var error = errorProp?.GetValue(response);
        var msgProp = error?.GetType().GetProperty("Message");
        return msgProp?.GetValue(error)?.ToString() ?? "Unknown error";
    }
}
