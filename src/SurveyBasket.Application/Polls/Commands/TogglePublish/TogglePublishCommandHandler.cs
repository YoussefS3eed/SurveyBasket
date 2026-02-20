using SurveyBasket.Application.Polls.Commands.TogglePublishPoll;

namespace SurveyBasket.Application.Polls.Commands.TogglePublish;

public class TogglePublishCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<TogglePublishCommand, Unit>
{
    public async Task<Unit> Handle(TogglePublishCommand request, CancellationToken cancellationToken)
    {
        var toggled = await pollRepository.TogglePublishAsync(request.Id, cancellationToken);
        if (!toggled)
            throw new PollNotFoundException(request.Id);

        return Unit.Value;
    }
}