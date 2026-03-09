using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Commands.UpdatePoll;

public class UpdatePollCommandHandler(
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePollCommand, Result>
{
    public async Task<Result> Handle(
        UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var exists = await pollRepository.ExistsByTitleExceptIdAsync(
            request.Title, request.Id, cancellationToken);

        if (exists)
            return Result.Failure(PollErrors.DuplicateTitle(request.Title));

        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound(request.Id));

        request.Adapt(poll);

        pollRepository.Update(poll);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}