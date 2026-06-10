using StarterKit.Domain.Entities;

namespace StarterKit.UseCase.Abstractions;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
