using Microsoft.Extensions.Caching.Hybrid;
using SurveyBasket.Application.Common.Caching;
using System.Collections.Concurrent;

namespace SurveyBasket.Infrastructure.Services.Cache;

public class HybridCacheService(HybridCache cache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, HashSet<string>> _keyMappings = new();

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

        // Track key by prefix (e.g., "polls:1:questions" for "polls:1:questions:page:1:...")
        var prefix = GetPrefix(key);
        _keyMappings.GetOrAdd(prefix, _ => []).Add(key);

        return await cache.GetOrCreateAsync(key, factory, options, cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
        RemoveKeyFromMapping(key);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        if (_keyMappings.TryGetValue(prefix, out var keys))
        {
            foreach (var key in keys)
                await cache.RemoveAsync(key, cancellationToken);

            _keyMappings.TryRemove(prefix, out _);
        }
    }

    public async Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        foreach (var key in keys)
        {
            await cache.RemoveAsync(key, cancellationToken);
            RemoveKeyFromMapping(key);
        }
    }

    private static string GetPrefix(string key)
    {
        // Extract base prefix: "polls:1:questions:page:1:size:10:..." → "polls:1:questions"
        var parts = key.Split(':');
        if (parts.Length >= 3)
            return string.Join(":", parts.Take(3));
        return key;
    }

    private static void RemoveKeyFromMapping(string key)
    {
        var prefix = GetPrefix(key);
        if (_keyMappings.TryGetValue(prefix, out var keys))
            keys.Remove(key);
    }
}