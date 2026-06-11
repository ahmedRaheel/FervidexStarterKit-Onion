using Dapper;
using MediatR;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.Domain.Shared.Pagination;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.UseCase.Handlers.Products.GetPaged;

public sealed record GetPagedProductsQuery(int PageNumber, int PageSize) : IRequest<PagedResult<ProductResponse>>;

public sealed class GetPagedProductsHandler(IProductQuery productQuery) : IRequestHandler<GetPagedProductsQuery, PagedResult<ProductResponse>>
{
    public async Task<PagedResult<ProductResponse>> Handle(GetPagedProductsQuery query, CancellationToken cancellationToken)
    {
        var response = await productQuery.GetProductsAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new PagedResult<ProductResponse>(response, query.PageNumber, query.PageSize, response.Count);
    }
}

