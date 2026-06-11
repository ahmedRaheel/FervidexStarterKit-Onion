using StarterKit.Domain.DTO;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Queries;

namespace UnitTests.UseCase.TestDoubles;

internal sealed class InMemoryProductQuery : IProductQuery
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
