using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StarterKit.Infrastructure.External.Caching.Redis;
using StarterKit.UseCase.Abstractions;

namespace StarterKit.Infrastructure.External.DependencyInjection;

public static class InfrastructureExternalDependencyInjection
{
    public static IServiceCollection AddInfrastructureExternal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis")
                ?? configuration["Redis:ConnectionString"]
                ?? "localhost:6379";
            options.InstanceName = "StarterKit:";
        });

        services.AddSingleton<IRedisCacheService, RedisCacheService>();
        services.AddHttpClient("resilient");
        return services;
    }
}
