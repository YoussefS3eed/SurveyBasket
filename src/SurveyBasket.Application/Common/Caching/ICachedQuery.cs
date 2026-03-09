namespace SurveyBasket.Application.Common.Caching;

public interface ICachedQuery
{
    string CacheKey { get; }
    TimeSpan? Expiration { get; }
    TimeSpan? LocalCacheExpiration { get; }
}

public interface ICachedQuery<TResponse> : IRequest<TResponse>, ICachedQuery
{
}