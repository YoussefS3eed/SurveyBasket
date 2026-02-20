namespace SurveyBasket.Application.Polls.Dtos;

public sealed record PollRequestDto
    (
        string Title,
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt
    );
