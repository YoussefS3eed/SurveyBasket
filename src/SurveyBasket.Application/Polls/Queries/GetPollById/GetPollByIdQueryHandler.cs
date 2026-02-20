namespace SurveyBasket.Application.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetPollByIdQuery, PollResponseDto>
{
    public async Task<PollResponseDto> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);
        if (poll is null)
            throw new PollNotFoundException(request.Id);

        return poll.Adapt<PollResponseDto>();
    }
}