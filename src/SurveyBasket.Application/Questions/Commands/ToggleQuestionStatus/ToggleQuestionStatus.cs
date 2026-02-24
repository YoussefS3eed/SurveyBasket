namespace SurveyBasket.Application.Questions.Commands.ToggleQuestionStatus;

public record ToggleQuestionStatusCommand(int PollId, int Id) : IRequest<Result>;
