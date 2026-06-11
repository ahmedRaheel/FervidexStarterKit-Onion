using FluentValidation;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.Domain.Shared.Results;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Command.Update;

public sealed record UpdateProductCommand(Guid Id, string Name, decimal Price, string Sku) : IRequest<Result<ProductResponse>>;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
    }
}

public sealed class UpdateProductHandler(IProductCommand productCommands, IProductQuery productQuery) : IRequestHandler<UpdateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        // 1. Fetch: Use the query service (Dapper/Optimized)
        var product = await productQuery.GetAsync(command.Id, ct);

        // 2. Return Result instead of throwing an exception
        if (product is null)
        {
            return Errors.NotFound($"Product with ID {command.Id} was not found.");
        }

        // 3. Logic: Update the Rich Domain Model
        product.Update(command.Name, command.Price, command.Sku);

        // 4. Persistence: Update via EF Core (Command Side)
        await productCommands.UpdateAsync(command.Id, product, ct);

        // 5. Response: Return success
        var response = new ProductResponse(product.Id, product.Name, product.Price, product.Sku, product.CreatedAtUtc);
        return Result<ProductResponse>.Success(response, "Product successfully updated.");
    }
}