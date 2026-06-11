using StarterKit.Domain.DTO.API;
using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces.Commands;
using StarterKit.UseCase.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterKit.Infrastructure.Data.Commands;

public sealed class ProductCommand (IApplicationDbContext dbContext): IProductCommand
{
    // Implement the methods defined in IProductCommands interface
    public async Task AddAsync(Product request, CancellationToken cancellationToken = default)
    {        
        dbContext.Products.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    // Implement the DeleteAsync method to delete a product by its ID
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.FindAsync(id, cancellationToken);
        if (product is not null)
        {
            dbContext.Products.Remove(product);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    // Implement the UpdateAsync method to update a product by its ID
    public async Task UpdateAsync(Guid id, Product request, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.FindAsync(id);
        if (product is not null)
        {
            product = request;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
