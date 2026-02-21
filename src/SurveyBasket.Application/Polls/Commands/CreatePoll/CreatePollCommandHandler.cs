using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler(IPollRepository pollRepository)
    : IRequestHandler<CreatePollCommand, Result<PollDto>>
{
    public async Task<Result<PollDto>> Handle(CreatePollCommand request, CancellationToken cancellationToken)
    {
        var poll = request.Adapt<Poll>();
        var createdPoll = await pollRepository.CreateAsync(poll, cancellationToken);
        return Result.Success(createdPoll.Adapt<PollDto>());
    }
}