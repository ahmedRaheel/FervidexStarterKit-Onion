using FluentAssertions;
using StarterKit.Domain.Constants;
using StarterKit.Domain.DTO;
using StarterKit.UseCase.Handlers.Products.Query.GetById;
using UnitTests.UseCase.TestDoubles;
using Xunit;

namespace UnitTests.UseCase.Products.Query.GetById;

public sealed class GetProductByIdHandlerTests
{
    [Fact]
    public async Task Handle_should_return_product_when_product_exists()
    {
        var id = Guid.NewGuid();
        var expected = new ProductResponse(id, "Product", 10m, "SKU-001", DateTime.UtcNow);
        var productQuery = new InMemoryProductQuery { ExistingResponse = expected };
        var handler = new GetProductByIdHandler(productQuery);

        var result = await handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_should_return_not_found_when_product_does_not_exist()
    {
        var id = Guid.NewGuid();
        var productQuery = new InMemoryProductQuery { ExistingResponse = null };
        var handler = new GetProductByIdHandler(productQuery);

        var result = await handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(ErrorCodes.NotFound);
    }
}
