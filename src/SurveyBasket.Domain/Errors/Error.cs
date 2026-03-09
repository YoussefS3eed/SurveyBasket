namespace SurveyBasket.Domain.Errors;

public sealed record Error
{
    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }
    public IReadOnlyList<ValidationError> Errors { get; }

    private Error(string code, string description, ErrorType type, IEnumerable<ValidationError>? errors = null)
    {
        Code = code;
        Description = description;
        Type = type;
        Errors = errors?.ToList().AsReadOnly() ?? new List<ValidationError>().AsReadOnly();
    }


    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Validation);

    // Factory methods for clean creation
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    public static Error Validation(IEnumerable<ValidationError> errors) =>
        new("Validation.General", "One or more validation errors occurred", ErrorType.Validation, errors);

    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);
}

public record ValidationError(string PropertyName, string ErrorMessage);

public enum ErrorType
{
    None,
    Validation,      // 400
    Unauthorized,    // 401
    Forbidden,       // 403
    NotFound,        // 404
    Conflict,        // 409
    Failure          // 500
}