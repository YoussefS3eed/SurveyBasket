namespace SurveyBasket.Application.Polls.Queries.GetPollById;

public record GetPollByIdQuery(int Id) : IRequest<Result<PollDto>>;