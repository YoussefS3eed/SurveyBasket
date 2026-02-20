namespace SurveyBasket.Application.Polls.Commands.TogglePublishPoll;

public record TogglePublishCommand(int Id) : IRequest<Unit>;
