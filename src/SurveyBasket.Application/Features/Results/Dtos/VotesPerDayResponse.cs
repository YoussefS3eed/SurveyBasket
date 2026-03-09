namespace SurveyBasket.Application.Features.Results.Dtos;

public record VotesPerDayResponse(
    DateOnly Date,
    int NumberOfVotes
);