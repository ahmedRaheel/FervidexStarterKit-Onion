using StarterKit.Domain.DTO;
using StarterKit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterKit.Domain.Interfaces.Queries;

/// <summary>
/// Defines the contract for querying product-related data.
/// </summary>
public interface IProductQuery
{
    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Product> GetAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a paginated list of products.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyList<ProductResponse>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ProductResponse> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
