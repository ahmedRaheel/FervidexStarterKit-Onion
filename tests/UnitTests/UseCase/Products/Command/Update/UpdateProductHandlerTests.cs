using FluentAssertions;
using StarterKit.Domain.Constants;
using StarterKit.Domain.Entities;
using StarterKit.UseCase.Handlers.Products.Command.Update;
using UnitTests.UseCase.TestDoubles;
using Xunit;

namespace UnitTests.UseCase.Products.Command.Update;

public sealed class UpdateProductHandlerTests
{
    [Fact]
    public async Task Handle_should_update_existing_product_and_return_success_response()
    {
        var existingProduct = new Product("Old Product", 10m, "OLD-001");
        var productQuery = new InMemoryProductQuery { ExistingProduct = existingProduct };
        var productCommand = new InMemoryProductCommand();
        var handler = new UpdateProductHandler(productCommand, productQuery);
        var command = new UpdateProductCommand(existingProduct.Id, "New Product", 20m, "NEW-001");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be(command.Name);
        result.Value.Price.Should().Be(command.Price);
        result.Value.Sku.Should().Be(command.Sku);
        productCommand.UpdatedId.Should().Be(command.Id);
        productCommand.UpdatedProduct.Should().NotBeNull();
        productCommand.UpdatedProduct!.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_should_return_not_found_when_product_does_not_exist()
    {
        var productQuery = new InMemoryProductQuery { ExistingProduct = null };
        var productCommand = new InMemoryProductCommand();
        var handler = new UpdateProductHandler(productCommand, productQuery);
        var command = new UpdateProductCommand(Guid.NewGuid(), "Product", 10m, "SKU-001");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(ErrorCodes.NotFound);
        productCommand.UpdatedProduct.Should().BeNull();
    }
}
