using FluentAssertions;
using StarterKit.Domain.Entities;
using Xunit;

namespace UnitTests.Domain;

public sealed class ProductTests
{
    [Fact]
    public void Constructor_should_create_product_with_expected_values()
    {
        var product = new Product("API Starter", 99.99m, "SKU-001");

        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be("API Starter");
        product.Price.Should().Be(99.99m);
        product.Sku.Should().Be("SKU-001");
        product.CreatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_should_change_values_and_mark_updated()
    {
        var product = new Product("Old Name", 10m, "OLD-SKU");

        product.Update("New Name", 25m, "NEW-SKU");

        product.Name.Should().Be("New Name");
        product.Price.Should().Be(25m);
        product.Sku.Should().Be("NEW-SKU");
        product.UpdatedAtUtc.Should().NotBeNull();
    }
}
