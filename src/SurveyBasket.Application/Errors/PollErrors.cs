namespace SurveyBasket.Application.Errors;

public static class PollErrors
{
    public static readonly Error NotFound = new("Poll.NotFound", "Poll not found.");
    public static readonly Error Conflict = new("Poll.Conflict", "A poll with the same title already exists.");
}