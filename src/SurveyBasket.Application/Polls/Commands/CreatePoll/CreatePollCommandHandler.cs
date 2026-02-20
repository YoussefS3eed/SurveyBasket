using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler
    (
        //ILogger<CreatePollCommandHandler> logger,
        IPollRepository pollRepository
    )
    : IRequestHandler<CreatePollCommand, PollResponseDto>
{
    public async Task<PollResponseDto> Handle(CreatePollCommand request, CancellationToken cancellationToken)
    {
        //logger.LogInformation("Creating new poll: {@PollRequest}", request);
        var poll = request.PollRequestDto.Adapt<Poll>();
        var created = await pollRepository.AddAsync(poll, cancellationToken);
        return created.Adapt<PollResponseDto>();
    }
}
