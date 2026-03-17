using SurveyBasket.Application.Features.Votes.Dtos;

namespace SurveyBasket.Application.Features.Votes.Queries.GetAvailableQuestions;

public record GetAvailableQuestionsQuery(int PollId) : IRequest<Result<IEnumerable<AvailableQuestionResponse>>>;

