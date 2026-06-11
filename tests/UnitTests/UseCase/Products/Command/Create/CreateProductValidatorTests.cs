using FluentAssertions;
using StarterKit.UseCase.Handlers.Products.Command.Create;
using Xunit;

namespace UnitTests.UseCase.Products.Command.Create;

public sealed class CreateProductValidatorTests
{
    private readonly CreateProductValidator _validator = new();

    [Fact]
    public void Validate_should_pass_when_command_is_valid()
    {
        var command = new CreateProductCommand("Product", 10m, "SKU-001");

        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", 10, "SKU-001")]
    [InlineData("Product", 0, "SKU-001")]
    [InlineData("Product", 10, "")]
    public void Validate_should_fail_when_command_is_invalid(string name, decimal price, string sku)
    {
        var command = new CreateProductCommand(name, price, sku);

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
