using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarterKit.Domain.Interfaces.Queries;
using StarterKit.Infrastructure.Data.Persistence.Context;
using StarterKit.Infrastructure.Data.Queries;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.Infrastructure.Data.DependencyInjection;

public static class InfrastructureDataDependencyInjection
{
    public static IServiceCollection AddInfrastructureData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {            
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IProductQuery, ProductQuery>();
        return services;
    }
}
