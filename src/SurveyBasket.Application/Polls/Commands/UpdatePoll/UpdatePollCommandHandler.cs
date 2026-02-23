using SurveyBasket.Application.Errors;

namespace SurveyBasket.Application.Polls.Commands.UpdatePoll;

public class UpdatePollCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<UpdatePollCommand, Result>
{
    public async Task<Result> Handle(UpdatePollCommand request, CancellationToken cancellationToken)
    {
        var exists = await pollRepository.ExistsByTitleExceptIdAsync(request.Title, request.Id, cancellationToken);
        if (exists)
        {
            var error = PollErrors.Conflict with
            {
                Description = $"A poll with title '{request.Title}' already exists."
            };
            return Result.Failure<PollDto>(error);
        }

        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound);

        request.Adapt(poll);
        await pollRepository.UpdateAsync(poll, cancellationToken);

        return Result.Success();
    }
}