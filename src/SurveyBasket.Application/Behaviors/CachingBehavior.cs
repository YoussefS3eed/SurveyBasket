using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Abstractions.Caching;
using SurveyBasket.Application.Interfaces;
using System.Reflection;

namespace SurveyBasket.Application.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    IAppCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Extract the T from Result<T> and dispatch to the generic helper
        var dataType = typeof(TResponse).GetGenericArguments()[0];

        var method = GetType()
            .GetMethod(nameof(HandleCaching), BindingFlags.NonPublic | BindingFlags.Instance)!
            .MakeGenericMethod(dataType);

        return await (Task<TResponse>)method.Invoke(this, [request, next, cancellationToken])!;
    }

    private async Task<TResponse> HandleCaching<TData>(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) where TData : class
    {
        bool factoryWasCalled = false;
        TResponse? resultFromHandler = default;

        // GetOrCreateAsync stores TData (not Result<TData>).
        // HybridCache does NOT cache null, so returning null from the factory
        // on handler failure means failures are never persisted to cache.
        var data = await cache.GetOrCreateAsync(
            request.CacheKey,
            async ct =>
            {
                factoryWasCalled = true;
                resultFromHandler = await next(ct);

                if (resultFromHandler.IsSuccess && resultFromHandler is Result<TData> typed)
                {
                    logger.LogInformation("[Cache] Miss — storing data for key: {CacheKey}", request.CacheKey);
                    return typed.Value;
                }

                logger.LogInformation("[Cache] Miss — handler failed, skipping cache for key: {CacheKey}", request.CacheKey);
                return null!; // HybridCache will not store null, so failures are never cached
            },
            request.Expiration,
            request.LocalCacheExpiration,
            cancellationToken);

        // Cache hit: factory was NOT called and we got data back
        if (!factoryWasCalled && data is not null)
        {
            logger.LogInformation("[Cache] Hit for key: {CacheKey}", request.CacheKey);
            return (TResponse)(object)Result.Success(data);
        }

        // Cache miss: factory ran — return the handler result directly
        if (factoryWasCalled)
            return resultFromHandler!;

        // Fallback (shouldn't happen in practice)
        return await next(cancellationToken);
    }
}