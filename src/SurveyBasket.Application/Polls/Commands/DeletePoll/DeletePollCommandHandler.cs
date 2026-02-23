using SurveyBasket.Application.Errors;

namespace SurveyBasket.Application.Polls.Commands.DeletePoll;

public class DeletePollCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<DeletePollCommand, Result>
{
    public async Task<Result> Handle(DeletePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound);

        await pollRepository.DeleteAsync(poll, cancellationToken);
        return Result.Success();
    }
}