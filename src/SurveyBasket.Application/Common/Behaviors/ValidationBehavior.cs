using System.Reflection;

namespace SurveyBasket.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    // ✅ Cached once per generic instantiation — no reflection overhead per call
    private static readonly MethodInfo? _genericFailureMethod =
        typeof(TResponse).IsGenericType
            ? typeof(Result)
                .GetMethod(nameof(Result.Failure), 1, [typeof(Error)])!
                .MakeGenericMethod(typeof(TResponse).GetGenericArguments()[0])
            : null;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var failures = (await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))))
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
            .ToList();

        if (failures.Count == 0)
            return await next(cancellationToken);

        var error = Error.Validation(failures);
        return CreateFailureResult(error);
    }

    private static TResponse CreateFailureResult(Error error)
    {
        // Non-generic Result
        if (typeof(TResponse) == typeof(Result))
            return (TResponse)(object)Result.Failure(error);

        // Generic Result<T> — use cached MethodInfo
        if (_genericFailureMethod is not null)
            return (TResponse)_genericFailureMethod.Invoke(null, [error])!;

        throw new InvalidOperationException($"Unsupported result type: {typeof(TResponse).Name}");
    }
}