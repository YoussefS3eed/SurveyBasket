namespace SurveyBasket.Application.Polls.Commands.TogglePublish;

public record TogglePublishCommand(int Id) : IRequest<Result>;