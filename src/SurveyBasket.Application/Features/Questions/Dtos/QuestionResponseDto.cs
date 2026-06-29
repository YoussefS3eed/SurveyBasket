using SurveyBasket.Application.Features.Answers.Dtos;

namespace SurveyBasket.Application.Features.Questions.Dtos;

public record QuestionResponseDto(int Id, string Content, IEnumerable<AnswerResponseDto> Answers);
