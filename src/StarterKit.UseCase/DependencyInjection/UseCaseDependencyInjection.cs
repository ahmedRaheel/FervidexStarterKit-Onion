using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StarterKit.UseCase.Abstractions.Behaviors;
using System.Reflection;

namespace StarterKit.UseCase.DependencyInjection;

public static class UseCaseDependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        return services;
    }
}
