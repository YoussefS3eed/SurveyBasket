using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(int PollId, string Content, IEnumerable<string> Answers) : IRequest<Result<QuestionResponse>>;