using FluentValidation;
using MediatR;
namespace StarterKit.UseCase.Abstractions.Behaviors;
public sealed class ValidationBehavior<TRequest,TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest,TResponse> 
{ 
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = validators.Select(v => v.Validate(context)).SelectMany(r => r.Errors).ToList();

        if (failures is null)
            return await next(); // Proceed to the handler

        throw new ValidationException(failures);        
    }
}
