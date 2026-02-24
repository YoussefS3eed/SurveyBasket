namespace SurveyBasket.Application.Questions.Dtos;

public record QuestionRequest(string Content, List<string> Answers);
