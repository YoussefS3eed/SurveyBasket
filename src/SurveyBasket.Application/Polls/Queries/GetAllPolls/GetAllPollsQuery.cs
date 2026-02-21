namespace SurveyBasket.Application.Polls.Queries.GetAllPolls;

public record GetAllPollsQuery : IRequest<Result<IEnumerable<PollDto>>>;