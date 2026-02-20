namespace SurveyBasket.Application.Polls.Commands.DeletePoll;

public sealed record DeletePollCommand(int Id) : IRequest<Unit>;
