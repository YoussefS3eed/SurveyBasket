namespace SurveyBasket.Application.Features.Results.Dtos;

public record VotesPerAnswerResponseDto(
    string Answer,
    int Count
);