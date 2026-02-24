// SurveyBasket.Application/Questions/Commands/UpdateQuestion/UpdateQuestionCommand.cs
namespace SurveyBasket.Application.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(int PollId, int Id, string Content, List<string> Answers) : IRequest<Result>;
