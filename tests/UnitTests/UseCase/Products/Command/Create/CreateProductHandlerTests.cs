using FluentAssertions;
using StarterKit.UseCase.Handlers.Products.Command.Create;
using UnitTests.UseCase.TestDoubles;
using Xunit;

namespace UnitTests.UseCase.Products.Command.Create;

public sealed class CreateProductHandlerTests
{
    [Fact]
    public async Task Handle_should_persist_product_and_return_success_response()
    {
        var productCommand = new InMemoryProductCommand();
        var handler = new CreateProductHandler(productCommand);
        var command = new CreateProductCommand("Fervidex Starter Kit", 49m, "FRV-001");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be(command.Name);
        result.Value.Price.Should().Be(command.Price);
        result.Value.Sku.Should().Be(command.Sku);
        productCommand.AddedProduct.Should().NotBeNull();
        productCommand.AddedProduct!.Name.Should().Be(command.Name);
        productCommand.AddedProduct.Price.Should().Be(command.Price);
        productCommand.AddedProduct.Sku.Should().Be(command.Sku);
    }
}
