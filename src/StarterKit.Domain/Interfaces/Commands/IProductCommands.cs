using StarterKit.Domain.DTO.API;
using StarterKit.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterKit.Domain.Interfaces.Commands;

/// <summary>
/// Defines the contract for product-related command operations,
/// including adding, updating, and deleting products in the system.
/// </summary>
public interface IProductCommands
{
    /// <summary>
    /// Adds a new product to the system based on the provided CreateProductRequest.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(Product request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product identified by the provided id using the details from the UpdateProductRequest.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAsync(Guid id, Product request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the product identified by the provided id from the system.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
