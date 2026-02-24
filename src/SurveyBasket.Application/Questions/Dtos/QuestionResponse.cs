using SurveyBasket.Application.Answers.Dtos;

namespace SurveyBasket.Application.Questions.Dtos;

public record QuestionResponse(int Id, string Content, IEnumerable<AnswerResponse> Answers);
