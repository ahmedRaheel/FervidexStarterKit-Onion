using Dapper;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Shared.Pagination;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.GetPaged;

public sealed record GetPagedProductsQuery(int PageNumber, int PageSize) : IRequest<PagedResult<ProductResponse>>;

public sealed class GetPagedProductsHandler(IDbConnectionFactory factory) : IRequestHandler<GetPagedProductsQuery, PagedResult<ProductResponse>>
{
    public async Task<PagedResult<ProductResponse>> Handle(GetPagedProductsQuery query, CancellationToken cancellationToken)
    {
        using var connection = factory.CreateConnection();
        var offset = (query.PageNumber - 1) * query.PageSize;
        var items = (await connection.QueryAsync<ProductResponse>(
            "SELECT Id,Name,Price,Sku,CreatedAtUtc FROM Products WHERE IsDeleted=0 ORDER BY CreatedAtUtc DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY",
            new { Offset = offset, query.PageSize })).ToList();
        var total = await connection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM Products WHERE IsDeleted=0");
        return new PagedResult<ProductResponse>(items, query.PageNumber, query.PageSize, total);
    }
}
