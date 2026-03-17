using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Commands.TogglePollPublish;

public sealed class TogglePollPublishCommandHandler(
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork,
    IBackgroundJobService backgroundJobService)
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

        if (poll.IsPublished && poll.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
            backgroundJobService.Enqueue<INotificationService>(notificationService => notificationService.SendNewPollsNotification(poll.Id));

        return Result.Success();
    }
}