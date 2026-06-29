namespace SurveyBasket.Application.Features.Results.Dtos;

public record VotesPerDayResponseDto(
    DateOnly Date,
    int NumberOfVotes
);