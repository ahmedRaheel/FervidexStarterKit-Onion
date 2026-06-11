using FluentValidation;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Create;

public sealed record CreateProductCommand(string Name, decimal Price, string Sku) : IRequest<ProductResponse>;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
    }
}

public sealed class CreateProductHandler(IProductCommands productCommands) : IRequestHandler<CreateProductCommand, ProductResponse>
{
    public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product(command.Name, command.Price, command.Sku);
        await productCommands.AddAsync(product, cancellationToken);
        return new(product.Id, product.Name, product.Price, product.Sku, product.CreatedAtUtc);
    }
}
