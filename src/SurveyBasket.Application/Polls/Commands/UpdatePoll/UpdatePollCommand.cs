namespace SurveyBasket.Application.Polls.Commands.UpdatePoll;

public record UpdatePollCommand(int Id, PollRequestDto PollRequestDto) : IRequest<Unit>;
