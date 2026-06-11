using Dapper;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;

public sealed class GetProductByIdHandler(IProductQuery productQuery) : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    public async Task<ProductResponse?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        return await productQuery.GetProductByIdAsync(query.Id, cancellationToken);
    }
}
