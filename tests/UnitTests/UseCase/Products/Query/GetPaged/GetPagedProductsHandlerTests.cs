using FluentAssertions;
using StarterKit.Domain.DTO;
using StarterKit.UseCase.Handlers.Products.Query.GetPaged;
using UnitTests.UseCase.TestDoubles;
using Xunit;

namespace UnitTests.UseCase.Products.Query.GetPaged;

public sealed class GetPagedProductsHandlerTests
{
    [Fact]
    public async Task Handle_should_wrap_products_in_paged_result()
    {
        var products = new[]
        {
            new ProductResponse(Guid.NewGuid(), "Product A", 10m, "A-001", DateTime.UtcNow),
            new ProductResponse(Guid.NewGuid(), "Product B", 20m, "B-001", DateTime.UtcNow)
        };
        var productQuery = new InMemoryProductQuery { Products = products };
        var handler = new GetPagedProductsHandler(productQuery);

        var result = await handler.Handle(new GetPagedProductsQuery(1, 10), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Items.Should().HaveCount(2);
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
        result.Value.TotalCount.Should().Be(2);
    }
}
