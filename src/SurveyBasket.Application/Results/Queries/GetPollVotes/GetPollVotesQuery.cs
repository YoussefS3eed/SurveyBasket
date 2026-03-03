using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetPollVotes;

public record GetPollVotesQuery(int PollId) : IRequest<Result<PollVotesResponse>>;