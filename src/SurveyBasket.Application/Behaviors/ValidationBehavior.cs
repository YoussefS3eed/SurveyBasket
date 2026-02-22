namespace SurveyBasket.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next
        , CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            //var error = Error.Validation with
            //{
            //    Description = string.Join("; ", failures.Select(f => f.ErrorMessage))
            //};
            //var error = new Error("Error.Validation", string.Join("; ", failures.Select(f => f.ErrorMessage)));
            //return (dynamic)Result.Failure(error);
            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}