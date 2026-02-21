namespace SurveyBasket.Application.Polls.Dtos;

public record UpdatePollRequest(
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
);