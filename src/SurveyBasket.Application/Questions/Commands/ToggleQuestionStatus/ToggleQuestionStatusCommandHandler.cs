using SurveyBasket.Application.Questions.Dtos;

namespace SurveyBasket.Application.Questions.Commands.ToggleQuestionStatus;

internal class ToggleQuestionStatusCommandHandler(IQuestionRepository questionRepository, IPollRepository pollRepository)
    : IRequestHandler<ToggleQuestionStatusCommand, Result>
{
    public async Task<Result> Handle(ToggleQuestionStatusCommand request, CancellationToken cancellationToken)
    {
        var existPoll = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!existPoll)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var question = await questionRepository.GetByIdAsync(request.PollId, request.Id, false, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        question.IsActive = !question.IsActive;

        await questionRepository.UpdateAsync(question, cancellationToken);
        return Result.Success();
    }
}