using FluentValidation;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.Domain.Shared.Results;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.Create;

public sealed record CreateProductCommand(string Name, decimal Price, string Sku) : IRequest<Result<ProductResponse>>;

public sealed class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Sku).NotEmpty().MaximumLength(64);
    }
}

public sealed class CreateProductHandler(IProductCommand productCommands) : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // 1. Create the entity (Rich Domain Model)
        var product = new Product(command.Name, command.Price, command.Sku);

        // 2. Persist using EF Core (Command side)
        await productCommands.AddAsync(product, cancellationToken);

        // 3. Map to Response DTO
        var response = new ProductResponse(product.Id, product.Name, product.Price, product.Sku, product.CreatedAtUtc);

        // 4. Return wrapped Result
        return Result<ProductResponse>.Success(response, "Product successfully created.");
    }
}
