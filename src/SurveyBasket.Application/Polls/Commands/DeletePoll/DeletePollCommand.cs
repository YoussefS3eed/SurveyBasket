namespace SurveyBasket.Application.Polls.Commands.DeletePoll;

public record DeletePollCommand(int Id) : IRequest<Result>;