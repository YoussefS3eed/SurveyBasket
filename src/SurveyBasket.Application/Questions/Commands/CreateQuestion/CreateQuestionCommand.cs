using SurveyBasket.Application.Questions.Dtos;

namespace SurveyBasket.Application.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(int PollId, string Content, List<string> Answers) : IRequest<Result<QuestionResponse>>;