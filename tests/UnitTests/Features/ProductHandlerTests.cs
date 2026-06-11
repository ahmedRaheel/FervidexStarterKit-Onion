using FluentAssertions;
using MediatR;
using StarterKit.Domain.Constants;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.UseCase.Handlers.Products.Create;
using StarterKit.UseCase.Handlers.Products.Delete;
using StarterKit.UseCase.Handlers.Products.GetById;
using StarterKit.UseCase.Handlers.Products.GetPaged;
using StarterKit.UseCase.Handlers.Products.Update;
using Xunit;

namespace UnitTests.Features;

public sealed class ProductHandlerTests
{
    [Fact]
    public async Task CreateProductHandler_should_persist_product_and_return_response()
    {
        var commandStore = new InMemoryProductCommand();
        var handler = new CreateProductHandler(commandStore);

        var result = await handler.Handle(new CreateProductCommand("Starter Kit", 49m, "STARTER-001"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("Starter Kit");
        commandStore.AddedProduct.Should().NotBeNull();
        commandStore.AddedProduct!.Sku.Should().Be("STARTER-001");
    }

    [Fact]
    public async Task GetProductByIdHandler_should_return_not_found_when_product_does_not_exist()
    {
        var query = new InMemoryProductQuery();
        var handler = new GetProductByIdHandler(query);

        var result = await handler.Handle(new GetProductByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be(ErrorCodes.NotFound);
    }

    [Fact]
    public async Task UpdateProductHandler_should_update_existing_product()
    {
        var existing = new Product("Old", 10m, "OLD");
        var query = new InMemoryProductQuery { ExistingProduct = existing };
        var commandStore = new InMemoryProductCommand();
        var handler = new UpdateProductHandler(commandStore, query);

        var result = await handler.Handle(new UpdateProductCommand(existing.Id, "New", 20m, "NEW"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        commandStore.UpdatedProduct.Should().NotBeNull();
        commandStore.UpdatedProduct!.Name.Should().Be("New");
        commandStore.UpdatedId.Should().Be(existing.Id);
    }

    [Fact]
    public async Task DeleteProductHandler_should_delete_existing_product()
    {
        var id = Guid.NewGuid();
        var query = new InMemoryProductQuery
        {
            ExistingResponse = new ProductResponse(id, "Product", 10m, "SKU", DateTime.UtcNow)
        };
        var commandStore = new InMemoryProductCommand();
        var handler = new DeleteProductHandler(commandStore, query);

        var result = await handler.Handle(new DeleteProductCommand(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(Unit.Value);
        commandStore.DeletedId.Should().Be(id);
    }

    [Fact]
    public async Task GetPagedProductsHandler_should_wrap_products_in_paged_result()
    {
        var query = new InMemoryProductQuery
        {
            Products =
            [
                new ProductResponse(Guid.NewGuid(), "A", 10m, "A-001", DateTime.UtcNow),
                new ProductResponse(Guid.NewGuid(), "B", 20m, "B-001", DateTime.UtcNow)
            ]
        };
        var handler = new GetPagedProductsHandler(query);

        var result = await handler.Handle(new GetPagedProductsQuery(1, 10), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Items.Should().HaveCount(2);
        result.Value.PageNumber.Should().Be(1);
        result.Value.PageSize.Should().Be(10);
    }

    private sealed class InMemoryProductCommand : IProductCommand
    {
        public Product? AddedProduct { get; private set; }
        public Guid? UpdatedId { get; private set; }
        public Product? UpdatedProduct { get; private set; }
        public Guid? DeletedId { get; private set; }

        public Task AddAsync(Product request, CancellationToken cancellationToken = default)
        {
            AddedProduct = request;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Guid id, Product request, CancellationToken cancellationToken = default)
        {
            UpdatedId = id;
            UpdatedProduct = request;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            DeletedId = id;
            return Task.CompletedTask;
        }
    }

    private sealed class InMemoryProductQuery : IProductQuery
    {
        public Product? ExistingProduct { get; init; }
        public ProductResponse? ExistingResponse { get; init; }
        public IReadOnlyList<ProductResponse> Products { get; init; } = [];

        public Task<Product> GetAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(ExistingProduct!);

        public Task<ProductResponse> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
            Task.FromResult(ExistingResponse!);

        public Task<IReadOnlyList<ProductResponse>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default) =>
            Task.FromResult(Products);
    }
}
