using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetVotesPerQuestion;

public record GetVotesPerQuestionQuery(int PollId) : IRequest<Result<IEnumerable<VotesPerQuestionResponse>>>;