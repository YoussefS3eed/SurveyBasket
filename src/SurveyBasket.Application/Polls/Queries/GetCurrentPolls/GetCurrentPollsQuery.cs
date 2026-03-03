namespace SurveyBasket.Application.Polls.Queries.GetCurrentPolls;

public record GetCurrentPollsQuery : IRequest<Result<IEnumerable<PollDto>>>;