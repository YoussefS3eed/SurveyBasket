using SurveyBasket.Application.Features.Votes.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Votes.Queries.GetAvailableQuestions;

public record GetAvailableQuestionsQuery(int PollId) : IRequest<Result<IEnumerable<AvailableQuestionResponse>>>;

