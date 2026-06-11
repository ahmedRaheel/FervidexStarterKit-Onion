using FluentAssertions;
using MediatR;
using StarterKit.Domain.Constants;
using StarterKit.Domain.DTO;
using StarterKit.UseCase.Handlers.Products.Command.Delete;
using UnitTests.UseCase.TestDoubles;
using Xunit;

namespace UnitTests.UseCase.Products.Command.Delete;

public sealed class DeleteProductHandlerTests
{
    [Fact]
    public async Task Handle_should_delete_existing_product_and_return_success()
    {
        var id = Guid.NewGuid();
        var productQuery = new InMemoryProductQuery
        {
            ExistingResponse = new ProductResponse(id, "Product", 10m, "SKU-001", DateTime.UtcNow)
        };
        var productCommand = new InMemoryProductCommand();
        var handler = new DeleteProductHandler(productCommand, productQuery);

        var result = await handler.Handle(new DeleteProductCommand(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        productCommand.DeletedId.Should().Be(id);
    }

    [Fact]
    public async Task Handle_should_return_not_found_when_product_does_not_exist()
    {
        var id = Guid.NewGuid();
        var productQuery = new InMemoryProductQuery { ExistingResponse = null };
        var productCommand = new InMemoryProductCommand();
        var handler = new DeleteProductHandler(productCommand, productQuery);

        var result = await handler.Handle(new DeleteProductCommand(id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(ErrorCodes.NotFound);
        productCommand.DeletedId.Should().BeNull();
    }
}
