namespace SurveyBasket.Application.Features.Results.Dtos;

public record VotesPerAnswerResponse(
    string Answer,
    int Count
);