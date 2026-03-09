using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(int PollId, string Content, List<string> Answers) : IRequest<Result<QuestionResponse>>;