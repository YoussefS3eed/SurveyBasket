using SurveyBasket.Application.Questions.Dtos;

namespace SurveyBasket.Application.Questions.Queries.GetQuestionById;

public record GetQuestionByIdQuery(int PollId, int Id) : IRequest<Result<QuestionResponse>>;