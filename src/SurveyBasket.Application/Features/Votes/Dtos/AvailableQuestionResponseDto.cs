using SurveyBasket.Application.Features.Answers.Dtos;

namespace SurveyBasket.Application.Features.Votes.Dtos;

public record AvailableQuestionResponseDto(
    int Id,
    string Content,
    IEnumerable<AnswerResponseDto> Answers
);