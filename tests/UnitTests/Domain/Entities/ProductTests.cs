using FluentAssertions;
using StarterKit.Domain.Entities;
using Xunit;

namespace UnitTests.Domain.Entities;

public sealed class ProductTests
{
    [Fact]
    public void Constructor_should_create_product_with_expected_values()
    {
        var product = new Product("Product", 10m, "SKU-001");

        product.Name.Should().Be("Product");
        product.Price.Should().Be(10m);
        product.Sku.Should().Be("SKU-001");
    }

    [Fact]
    public void Update_should_change_product_values()
    {
        var product = new Product("Old", 10m, "OLD-001");

        product.Update("New", 20m, "NEW-001");

        product.Name.Should().Be("New");
        product.Price.Should().Be(20m);
        product.Sku.Should().Be("NEW-001");
    }
}
