using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.Application.Interfaces;

namespace SurveyBasket.Infrastructure.Implementation.Caching;

public class HybridAppCache(HybridCache cache) : IAppCache
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

        return await cache.GetOrCreateAsync(
            key,
            async ct => await factory(ct),
            options,
            cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => await cache.RemoveAsync(key, cancellationToken);
}