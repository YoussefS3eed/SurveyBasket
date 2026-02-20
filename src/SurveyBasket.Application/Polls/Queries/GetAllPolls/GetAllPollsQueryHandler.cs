namespace SurveyBasket.Application.Polls.Queries.GetAllPolls;

public class GetAllPollsQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetAllPollsQuery, IEnumerable<PollResponseDto>>
{
    public async Task<IEnumerable<PollResponseDto>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetAllAsync(cancellationToken);
        return polls.Adapt<IEnumerable<PollResponseDto>>();
    }
}
