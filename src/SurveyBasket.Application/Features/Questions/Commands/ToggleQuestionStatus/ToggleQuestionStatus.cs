namespace SurveyBasket.Application.Features.Questions.Commands.ToggleQuestionStatus;

public record ToggleQuestionStatusCommand(int PollId, int Id) : IRequest<Result>;
