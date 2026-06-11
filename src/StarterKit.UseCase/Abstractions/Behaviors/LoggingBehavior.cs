using MediatR;
using Microsoft.Extensions.Logging;
using StarterKit.Domain.Shared.Results;
namespace StarterKit.UseCase.Abstractions.Behaviors;
public sealed class LoggingBehavior<TRequest,TResponse>(ILogger<LoggingBehavior<TRequest,TResponse>> logger) : IPipelineBehavior<TRequest,TResponse> 
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        logger.LogInformation("Processing {RequestName}", typeof(TRequest).Name);

        var response = await next();

        // Check if the response is of type Result<T> using reflection or pattern matching
        if (response is IResult { IsFailure: true } result)
        {
            logger.LogError("Request {RequestName} failed: {Error}", typeof(TRequest).Name, result.Error.Message);
        }
        else
        {
            logger.LogInformation("Processed {RequestName} successfully", typeof(TRequest).Name);
        }

        return response;
    }    
}
