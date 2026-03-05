namespace SurveyBasket.Application.Abstractions.Caching;

public interface IInvalidateCacheCommand
{
    IEnumerable<string> CacheKeys { get; }
}

public interface IInvalidateCacheCommand<TResponse> : IRequest<TResponse>, IInvalidateCacheCommand
{
}