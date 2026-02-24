namespace SurveyBasket.Application.Polls.Commands.TogglePollPublish;

public record TogglePollPublishCommand(int Id) : IRequest<Result>;