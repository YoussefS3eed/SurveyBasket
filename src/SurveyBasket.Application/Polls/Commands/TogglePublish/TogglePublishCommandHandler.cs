using SurveyBasket.Application.Errors;

namespace SurveyBasket.Application.Polls.Commands.TogglePublish;

public class TogglePublishHandler(IPollRepository pollRepository)
    : IRequestHandler<TogglePublishCommand, Result>
{
    public async Task<Result> Handle(TogglePublishCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound);

        poll.IsPublished = !poll.IsPublished;
        await pollRepository.UpdateAsync(poll, cancellationToken);

        return Result.Success();
    }
}