using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.Application.Common.Caching;

namespace SurveyBasket.Infrastructure.Services.Cache;

public class HybridCacheService(HybridCache cache) : ICacheService
{
    public async Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan? expiration = null,
        TimeSpan? localCacheExpiration = null,
        CancellationToken cancellationToken = default)
    {
        var options = new HybridCacheEntryOptions
        {
            Expiration = expiration,
            LocalCacheExpiration = localCacheExpiration ?? expiration
        };

        return await cache.GetOrCreateAsync(key, factory, options, cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => await cache.RemoveAsync(key, cancellationToken);

    // ✅ Bulk invalidation
    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        foreach (var key in keys)
            await cache.RemoveAsync(key, cancellationToken);
    }
}