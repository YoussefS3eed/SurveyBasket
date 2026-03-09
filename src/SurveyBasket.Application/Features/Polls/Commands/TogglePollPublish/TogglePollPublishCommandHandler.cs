using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Commands.TogglePollPublish;

public sealed class TogglePollPublishCommandHandler(
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<TogglePollPublishCommand, Result>
{
    public async Task<Result> Handle(
        TogglePollPublishCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound(request.Id));

        poll.IsPublished = !poll.IsPublished;

        pollRepository.Update(poll);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}