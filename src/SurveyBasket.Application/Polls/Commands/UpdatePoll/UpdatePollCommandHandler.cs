namespace SurveyBasket.Application.Polls.Commands.UpdatePoll;

public class UpdatePollCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<UpdatePollCommand, Unit>
{
    public async Task<Unit> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);
        if (poll is null)
            throw new PollNotFoundException(request.Id);

        request.PollRequestDto.Adapt(poll);
        await pollRepository.UpdateAsync(poll, cancellationToken);

        return Unit.Value;
    }
}