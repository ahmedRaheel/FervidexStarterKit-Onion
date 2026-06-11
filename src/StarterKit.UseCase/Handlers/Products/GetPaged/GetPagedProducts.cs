using Dapper;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.Domain.Shared.Pagination;
using StarterKit.Domain.Shared.Results;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.GetPaged;

public sealed record GetPagedProductsQuery(int PageNumber, int PageSize) : IRequest<Result<PagedResult<ProductResponse>>>;

public sealed class GetPagedProductsHandler(IProductQuery productQuery) : IRequestHandler<GetPagedProductsQuery, Result<PagedResult<ProductResponse>>>
{
    public async Task<Result<PagedResult<ProductResponse>>> Handle(GetPagedProductsQuery query, CancellationToken ct)
    {
        // 1. Fetch data from infrastructure
        var items = await productQuery.GetProductsAsync(query.PageNumber, query.PageSize, ct);

        // 2. Wrap the paged result in your Result<T> pattern
        var pagedResult = new PagedResult<ProductResponse>(
            items,
            query.PageNumber,
            query.PageSize,
            items.Count // Assuming your repository returns the total count
        );

        // 3. Return via the enterprise Result pattern
        return Result<PagedResult<ProductResponse>>.Success(pagedResult, "Products retrieved successfully.");
    }
}

