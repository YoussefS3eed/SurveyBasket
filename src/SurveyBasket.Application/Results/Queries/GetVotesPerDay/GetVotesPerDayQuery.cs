using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetVotesPerDay;

public record GetVotesPerDayQuery(int PollId) : IRequest<Result<IEnumerable<VotesPerDayResponse>>>;