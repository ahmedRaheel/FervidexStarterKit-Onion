using FluentAssertions;
using StarterKit.UseCase.Handlers.Products.Create;
using StarterKit.UseCase.Handlers.Products.Update;
using Xunit;

namespace UnitTests.Features;

public sealed class ProductValidatorTests
{
    [Fact]
    public void CreateProductValidator_should_accept_valid_command()
    {
        var validator = new CreateProductValidator();
        var result = validator.Validate(new CreateProductCommand("Product", 10m, "SKU-001"));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", 10, "SKU-001")]
    [InlineData("Product", 0, "SKU-001")]
    [InlineData("Product", 10, "")]
    public void CreateProductValidator_should_reject_invalid_command(string name, decimal price, string sku)
    {
        var validator = new CreateProductValidator();
        var result = validator.Validate(new CreateProductCommand(name, price, sku));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UpdateProductValidator_should_accept_valid_command()
    {
        var validator = new UpdateProductValidator();
        var result = validator.Validate(new UpdateProductCommand(Guid.NewGuid(), "Product", 10m, "SKU-001"));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void UpdateProductValidator_should_reject_empty_name()
    {
        var validator = new UpdateProductValidator();
        var result = validator.Validate(new UpdateProductCommand(Guid.NewGuid(), "", 10m, "SKU-001"));

        result.IsValid.Should().BeFalse();
    }
}
