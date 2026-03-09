using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Common.Behaviors;

//public class CacheInvalidationBehavior<TRequest, TResponse>(
//    ICacheService cache,
//    ILogger<CacheInvalidationBehavior<TRequest, TResponse>> logger)
//    : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : IRequest<TResponse>
//{
//    public async Task<TResponse> Handle(
//        TRequest request,
//        RequestHandlerDelegate<TResponse> next,
//        CancellationToken cancellationToken)
//    {
//        var response = await next(cancellationToken);

//        if (request is ICacheInvalidationCommand invalidateCommand)
//        {
//            foreach (var key in invalidateCommand.CacheKeys)
//            {
//                await cache.RemoveAsync(key, cancellationToken);
//                logger.LogInformation("[Cache] Invalidated key: {Key}", key);
//            }
//        }

//        return response;
//    }
//}


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

        // ✅ Only invalidate when the command succeeded
        if (request is ICacheInvalidationCommand invalidateCommand
            && response is Result { IsSuccess: true })
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