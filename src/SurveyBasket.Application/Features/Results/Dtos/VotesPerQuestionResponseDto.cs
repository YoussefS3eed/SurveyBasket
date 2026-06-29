namespace SurveyBasket.Application.Features.Results.Dtos;

public record VotesPerQuestionResponseDto(
    string Question,
    IEnumerable<VotesPerAnswerResponseDto> SelectedAnswers
);