using FluentValidation;
using MediatR;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Update;

public sealed record UpdateProductCommand(Guid Id, string Name, decimal Price, string Sku) : IRequest;

public sealed class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
    }
}

public sealed class UpdateProductHandler(IProductCommands productCommands, IProductQuery productQuery) : IRequestHandler<UpdateProductCommand>
{
    public async Task Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productQuery.GetAsync(command.Id, cancellationToken)
                      ?? throw new KeyNotFoundException("Product not found");

        product.Update(command.Name, command.Price, command.Sku);
        await productCommands.UpdateAsync(command.Id, product, cancellationToken);
    }
.