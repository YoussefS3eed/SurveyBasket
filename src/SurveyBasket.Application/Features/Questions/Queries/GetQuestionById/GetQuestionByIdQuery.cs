using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionById;

public record GetQuestionByIdQuery(int PollId, int Id) : IRequest<Result<QuestionResponse>>;