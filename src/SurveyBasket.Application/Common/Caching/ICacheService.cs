namespace SurveyBasket.Application.Common.Caching;

public interface ICacheService
{
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan? expiration = null,
        TimeSpan? localCacheExpiration = null,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    // ✅ Added for bulk invalidation
    Task RemoveManyAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default);
}