using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Abstractions.Caching;
using SurveyBasket.Application.Interfaces;

namespace SurveyBasket.Application.Behaviors;

public class CacheInvalidationBehavior<TRequest, TResponse>(
    IAppCache cache,
    ILogger<CacheInvalidationBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);

        if (request is IInvalidateCacheCommand invalidateCommand)
        {
            foreach (var key in invalidateCommand.CacheKeys)
            {
                await cache.RemoveAsync(key, cancellationToken);
                logger.LogInformation("[Cache] Invalidated key: {Key}", key);
            }
        }

        return response;
    }
}