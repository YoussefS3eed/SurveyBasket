namespace SurveyBasket.Application.Polls.Dtos;

public record PollResponseDto(
    int Id,
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
);