using SurveyBasket.Application.Features.Answers.Dtos;

namespace SurveyBasket.Application.Features.Questions.Dtos;

public record QuestionResponse(int Id, string Content, IEnumerable<AnswerResponse> Answers);
