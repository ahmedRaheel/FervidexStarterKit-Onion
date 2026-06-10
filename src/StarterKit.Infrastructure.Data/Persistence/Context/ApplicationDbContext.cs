using StarterKit.Domain.Entities;
using StarterKit.Domain.Interfaces;
using StarterKit.UseCase.Abstractions;
using System.Reflection;

namespace StarterKit.Infrastructure.Data.Persistence.Context;

public partial class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInformation();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditInformation();
        return base.SaveChanges();
    }

    private void ApplyAuditInformation()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetCreated(TimeProvider.System);
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.SetUpdated(TimeProvider.System);
            }
        }
    }
}
