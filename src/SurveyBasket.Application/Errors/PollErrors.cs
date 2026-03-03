namespace SurveyBasket.Application.Errors;

public static class PollErrors
{
    public static readonly Error PollNotFound = new("Poll.NotFound", "Poll not found.", 404);
    public static readonly Error Conflict = new("Poll.Conflict", "A poll with the same title already exists.", 409);
}