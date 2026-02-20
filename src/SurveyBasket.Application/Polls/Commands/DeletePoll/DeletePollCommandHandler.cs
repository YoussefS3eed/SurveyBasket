namespace SurveyBasket.Application.Polls.Commands.DeletePoll;

internal class DeletePollCommandHandler
    (
        //ILogger<DeletePollCommandHandler> logger,
        IPollRepository pollRepository
    )
    : IRequestHandler<DeletePollCommand, Unit>
{
    public async Task<Unit> Handle(DeletePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);
        if (poll is null)
            throw new PollNotFoundException(request.Id);

        await pollRepository.DeleteAsync(poll, cancellationToken);
        return Unit.Value;
    }
}
