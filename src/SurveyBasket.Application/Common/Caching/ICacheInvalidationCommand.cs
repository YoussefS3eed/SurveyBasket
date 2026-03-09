namespace SurveyBasket.Application.Common.Caching;

public interface ICacheInvalidationCommand
{
    IEnumerable<string> CacheKeys { get; }
}

// ✅ Consistent naming — same prefix as non-generic
public interface ICacheInvalidationCommand<TResponse>
    : IRequest<TResponse>, ICacheInvalidationCommand
{
}