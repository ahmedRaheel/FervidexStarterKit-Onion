using MediatR;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest;

public sealed class DeleteProductHandler(IProductCommands productCommands, IProductQuery productQuery) : IRequestHandler<DeleteProductCommand>
{
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productQuery.GetProductByIdAsync(command.Id, cancellationToken)
                      ?? throw new KeyNotFoundException("Product not found");

        await productCommands.DeleteAsync(command.Id,  cancellationToken);
    }
}
