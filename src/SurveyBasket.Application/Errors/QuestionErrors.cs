namespace SurveyBasket.Application.Errors;

public static class QuestionErrors
{
    public static readonly Error QuestionNotFound =
        new("Question.NotFound", "No question was found with the given ID", 404);

    public static readonly Error DuplicatedQuestionContent =
        new("Question.Conflict", "Another question with the same content already exists", 409);
}