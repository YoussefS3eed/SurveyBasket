namespace SurveyBasket.Application.Polls.Commands.UpdatePoll;

public record UpdatePollCommand(
    int Id,
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
) : IRequest<Result>;