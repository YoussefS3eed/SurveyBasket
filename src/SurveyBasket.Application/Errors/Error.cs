namespace SurveyBasket.Application.Errors;

public record Error(string Code, string Description, int? StatusCode)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", 400);
    public static readonly Error NotFound = new("Error.NotFound", "Resource not found", 404);
    public static readonly Error Validation = new("Error.Validation", "Validation failed", 400);
    public static readonly Error Unauthorized = new("Error.Unauthorized", "Unauthorized access", 401);
    //public static readonly Error Conflict = new("Error.Conflict", "A resource with the same data already exists", 409);
}