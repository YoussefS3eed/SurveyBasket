using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurveyBasket.Application.Interfaces;
using SurveyBasket.Infrastructure.Caching;

namespace SurveyBasket.Infrastructure.Extensions;

public static class CachingServiceExtensions
{
    public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        // Add HybridCache with in-memory only (no Redis)
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(10),
                Expiration = TimeSpan.FromMinutes(20)
            };

            //options.MaximumPayloadBytes = 1024 * 1024; // 1MB
            //options.MaximumKeyLength = 512;
        });

        // Register our cache abstraction
        services.AddScoped<IAppCache, HybridAppCache>();

        return services;
    }
}