using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Commands.DeletePoll;

public class DeletePollCommandHandler(
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeletePollCommand, Result>
{
    public async Task<Result> Handle(
        DeletePollCommand request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure(PollErrors.NotFound(request.Id));

        pollRepository.Delete(poll);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}