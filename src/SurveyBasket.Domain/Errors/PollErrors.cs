namespace SurveyBasket.Domain.Errors;

public static class PollErrors
{
    public static Error NotFound() =>
        Error.NotFound("Poll.NotFound", "Poll not found.");

    public static Error NotFound(object pollId) =>
        Error.NotFound("Poll.NotFound", $"Poll with id '{pollId}' was not found.");

    public static Error DuplicateTitle() =>
        Error.Conflict("Poll.DuplicateTitle", "A poll with the same title already exists.");

    public static Error DuplicateTitle(string title) =>
        Error.Conflict("Poll.DuplicateTitle", $"A poll with title '{title}' already exists.");
}