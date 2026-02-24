namespace SurveyBasket.Application.Polls.Commands.TogglePollPublish;

public class TogglePublishHandler(IPollRepository pollRepository)
    : IRequestHandler<TogglePollPublishCommand, Result>
{
    public async Task<Result> Handle(TogglePollPublishCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.PollNotFound);

        poll.IsPublished = !poll.IsPublished;
        await pollRepository.UpdateAsync(poll, cancellationToken);

        return Result.Success();
    }
}