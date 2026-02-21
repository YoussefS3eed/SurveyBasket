namespace SurveyBasket.Application.Common;

public record Error(string Code, string? Description)
{
    public static readonly Error None = new(string.Empty, null);
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided");
    public static readonly Error NotFound = new("Error.NotFound", "Resource not found");
    public static readonly Error Validation = new("Error.Validation", "Validation failed");
    public static readonly Error Unauthorized = new("Error.Unauthorized", "Unauthorized access");
    public static readonly Error Conflict = new("Error.Conflict", "A resource with the same data already exists");
}