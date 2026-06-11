using Dapper;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.Domain.Shared.Results;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductResponse?>>;

public sealed class GetProductByIdHandler(IProductQuery productQuery) : IRequestHandler<GetProductByIdQuery, Result<ProductResponse?>>
{
    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await productQuery.GetProductByIdAsync(query.Id, ct);

        if (product is null)
        {
            return Errors.NotFound($"Product with ID {query.Id} was not found.");
        }

        return Result<ProductResponse>.Success(product, "Product retrieved successfully.");
    }
}
