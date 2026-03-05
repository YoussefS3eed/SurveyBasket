namespace SurveyBasket.Application.Abstractions.Caching;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
    TimeSpan? LocalCacheExpiration { get; } // Optional: for local cache only
}

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery
    where TResponse : Result
{
}