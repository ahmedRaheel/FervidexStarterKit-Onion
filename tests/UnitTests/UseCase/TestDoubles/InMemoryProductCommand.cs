using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Commands;

namespace UnitTests.UseCase.TestDoubles;

internal sealed class InMemoryProductCommand : IProductCommand
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
