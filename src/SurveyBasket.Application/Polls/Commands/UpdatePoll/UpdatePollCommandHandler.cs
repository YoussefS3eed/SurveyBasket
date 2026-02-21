namespace SurveyBasket.Application.Polls.Commands.UpdatePoll;

public class UpdatePollCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<UpdatePollCommand, Result>
{
    public async Task<Result> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var exists = await pollRepository.ExistsByTitleAsync(request.Title, cancellationToken);
        if (exists)
        {
            var error = Error.Conflict with
            {
                Description = $"A poll with title '{request.Title}' already exists."
            };
            return Result.Failure<PollDto>(error);
        }

        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(Error.NotFound);

        request.Adapt(poll);
        await pollRepository.UpdateAsync(poll, cancellationToken);

        return Result.Success();
    }
}