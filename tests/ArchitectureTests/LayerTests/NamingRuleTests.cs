using FluentAssertions;
using FluentValidation;
using MediatR;
using StarterKit.UseCase.Handlers.Products.Create;
using Xunit;

namespace ArchitectureTests.LayerTests;

public sealed class NamingRuleTests
{
    [Fact]
    public void UseCase_request_handlers_should_have_handler_suffix()
    {
        var handlerTypes = typeof(CreateProductCommand).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .Where(type => type.GetInterfaces().Any(IsRequestHandlerInterface))
            .ToArray();

        handlerTypes.Should().NotBeEmpty();
        handlerTypes.Should().OnlyContain(type => type.Name.EndsWith("Handler", StringComparison.Ordinal));
    }

    [Fact]
    public void UseCase_validators_should_have_validator_suffix()
    {
        var validatorTypes = typeof(CreateProductCommand).Assembly
            .GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .Where(type => type.GetInterfaces().Any(IsValidatorInterface))
            .ToArray();

        validatorTypes.Should().NotBeEmpty();
        validatorTypes.Should().OnlyContain(type => type.Name.EndsWith("Validator", StringComparison.Ordinal));
    }

    private static bool IsRequestHandlerInterface(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequestHandler<,>);

    private static bool IsValidatorInterface(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IValidator<>);
}
