using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest;

public sealed class DeleteProductHandler(IApplicationDbContext db) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await db.Products.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken)
                      ?? throw new KeyNotFoundException("Product not found");

        db.Products.Remove(product);
        await db.SaveChangesAsync(cancellationToken);
    }
}
