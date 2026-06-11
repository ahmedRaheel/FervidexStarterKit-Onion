using MediatR;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.Domain.Shared.Results;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result<Unit>>;

public sealed class DeleteProductHandler(IProductCommands productCommands, IProductQuery productQuery) : IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        // 1. Check if it exists
        var product = await productQuery.GetProductByIdAsync(command.Id, ct);

        if (product is null)
        {
            return Errors.NotFound($"Product with ID {command.Id} was not found.");
        }

        // 2. Perform the deletion
        await productCommands.DeleteAsync(command.Id, ct);

        // 3. Return success (Unit is a common way to say "no data returned")
        return Result<Unit>.Success(Unit.Value, "Product successfully deleted.");
    }
}
