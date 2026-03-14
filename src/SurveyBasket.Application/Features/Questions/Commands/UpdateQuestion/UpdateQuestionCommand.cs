// SurveyBasket.Application/Questions/Commands/UpdateQuestion/UpdateQuestionCommand.cs
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(int PollId, int Id, string Content, IEnumerable<string> Answers) : IRequest<Result>;
