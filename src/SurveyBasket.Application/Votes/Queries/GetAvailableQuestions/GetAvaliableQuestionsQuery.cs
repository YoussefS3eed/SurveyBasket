using SurveyBasket.Application.Votes.Dtos;

namespace SurveyBasket.Application.Votes.Queries.GetAvailableQuestions;

public record GetAvailableQuestionsQuery(int PollId, string UserId) : IRequest<Result<IEnumerable<AvailableQuestionResponse>>>;
