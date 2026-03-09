using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Questions.Commands.ToggleQuestionStatus;

internal sealed class ToggleQuestionStatusCommandHandler(
    IQuestionRepository questionRepository,
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ToggleQuestionStatusCommand, Result>
{
    public async Task<Result> Handle(
        ToggleQuestionStatusCommand request, CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!pollExists)
            return Result.Failure(PollErrors.NotFound(request.PollId));

        var question = await questionRepository.GetByIdAsync(
            request.PollId, request.Id, false, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.NotFound());

        question.IsActive = !question.IsActive;

        questionRepository.Update(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}