using Dapper;
using StarterKit.Domain.DTO;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<ProductResponse?>;

public sealed class GetProductByIdHandler(IDbConnectionFactory factory) : IRequestHandler<GetProductByIdQuery, ProductResponse?>
{
    public async Task<ProductResponse?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        using var connection = factory.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<ProductResponse>(
            "SELECT Id,Name,Price,Sku,CreatedAtUtc FROM Products WHERE Id=@Id AND IsDeleted=0",
            new { query.Id });
    }
}
