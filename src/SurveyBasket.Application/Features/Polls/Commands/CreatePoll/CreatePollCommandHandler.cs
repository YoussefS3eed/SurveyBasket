using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler(
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePollCommand, Result<PollResponse>>
{
    public async Task<Result<PollResponse>> Handle(CreatePollCommand request, CancellationToken cancellationToken)
    {
        var exists = await pollRepository.ExistsByTitleExceptIdAsync(request.Title, null, cancellationToken);
        if (exists)
        {
            var error = PollErrors.DuplicateTitle(request.Title);
            return Result.Failure<PollResponse>(error);
        }

        var poll = request.Adapt<Poll>();

        await pollRepository.AddAsync(poll, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(poll.Adapt<PollResponse>());
    }
}