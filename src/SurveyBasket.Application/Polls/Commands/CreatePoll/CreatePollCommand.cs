namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public record CreatePollCommand(
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
) : IRequest<Result<PollDto>>;