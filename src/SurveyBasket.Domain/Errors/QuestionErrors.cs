namespace SurveyBasket.Domain.Errors;

public static class QuestionErrors
{
    public static Error NotFound() =>
        Error.NotFound("Question.NotFound", "No question was found with the given ID");

    public static Error NotFound(object questionId) =>
        Error.NotFound("Question.NotFound", $"Question with id '{questionId}' was not found.");

    public static Error DuplicatedQuestionContent =>
        Error.Conflict("Question.DuplicateContent", "Another question with the same content already exists");

    public static Error DuplicateContent(string content) =>
        Error.Conflict("Question.DuplicateContent", $"A question with content '{content}' already exists.");
}