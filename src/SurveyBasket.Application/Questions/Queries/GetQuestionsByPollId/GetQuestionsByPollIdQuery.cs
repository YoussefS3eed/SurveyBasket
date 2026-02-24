using SurveyBasket.Application.Questions.Dtos;

namespace SurveyBasket.Application.Questions.Queries.GetQuestionsByPollId;

public record GetQuestionsByPollIdQuery(int PollId) : IRequest<Result<IEnumerable<QuestionResponse>>>;