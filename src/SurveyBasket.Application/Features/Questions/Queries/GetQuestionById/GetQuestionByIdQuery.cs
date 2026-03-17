using SurveyBasket.Application.Features.Questions.Dtos;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionById;

public record GetQuestionByIdQuery(int PollId, int Id) : IRequest<Result<QuestionResponse>>;