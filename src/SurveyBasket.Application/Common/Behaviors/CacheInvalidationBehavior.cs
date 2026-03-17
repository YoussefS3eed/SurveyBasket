using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Caching;

namespace SurveyBasket.Application.Common.Behaviors;

public class CacheInvalidationBehavior<TRequest, TResponse>(
    ICacheService cache,
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

        if (request is ICacheInvalidationCommand invalidateCommand
            && response is Result { IsSuccess: true })
        {
            foreach (var key in invalidateCommand.CacheKeys)
            {
                await cache.RemoveByPrefixAsync(key, cancellationToken);
                logger.LogInformation("[Cache] Invalidated prefix: {Key}", key);
            }
        }

        return response;
    }
}