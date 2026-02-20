namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public sealed record CreatePollCommand(PollRequestDto PollRequestDto) : IRequest<PollResponseDto>;

