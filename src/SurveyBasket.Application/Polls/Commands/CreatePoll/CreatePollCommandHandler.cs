using SurveyBasket.Application.Errors;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<CreatePollCommand, Result<PollDto>>
{
    public async Task<Result<PollDto>> Handle(CreatePollCommand request, CancellationToken cancellationToken)
    {
        var exists = await pollRepository.ExistsByTitleAsync(request.Title, cancellationToken);
        if (exists)
        {
            var error = PollErrors.Conflict with
            {
                Description = $"A poll with title '{request.Title}' already exists."
            };
            return Result.Failure<PollDto>(error);
        }


        var poll = request.Adapt<Poll>();
        var createdPoll = await pollRepository.CreateAsync(poll, cancellationToken);
        return Result.Success(createdPoll.Adapt<PollDto>());
    }
}