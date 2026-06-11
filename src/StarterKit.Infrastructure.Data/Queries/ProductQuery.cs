using Dapper;
using StarterKit.Domain.DTO;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.UseCase.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterKit.Infrastructure.Data.Queries;

public sealed class ProductQuery(IDbConnectionFactory dbConnectionFactory) : IProductQuery
{
    /// <inheritdoc/>    
    public async Task<Product> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = "SELECT Id,Name,Price,Sku,CreatedAtUtc FROM Products WHERE Id=@Id";
        using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        var response = await connection.QuerySingleOrDefaultAsync<Product>(query, new { Id = id }).ConfigureAwait(false); 
        return response ?? null;
    }

    /// <inheritdoc/>    
    public async Task<ProductResponse> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var query = "SELECT Id,Name,Price,Sku,CreatedAtUtc FROM Products WHERE Id=@Id AND IsDeleted=0";
        using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);       
        var response = connection.QuerySingleOrDefault<ProductResponse>(query, new { Id = id });
        return response ?? null; 
    }

    /// <inheritdoc/>    
    public async Task<IReadOnlyList<ProductResponse>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);
        var offset = (page - 1) * pageSize;
        var query = "SELECT Id,Name,Price,Sku,CreatedAtUtc FROM Products WHERE IsDeleted=0 ORDER BY CreatedAtUtc DESC LIMIT @Limit OFFSET @Offset";
        var response = await connection.QueryAsync<ProductResponse>(query, new { Limit = pageSize, Offset = offset });
        return response.ToList();
    }
}