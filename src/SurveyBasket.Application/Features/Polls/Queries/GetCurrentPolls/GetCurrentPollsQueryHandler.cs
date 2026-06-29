using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetCurrentPolls;

public class GetCurrentPollsQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetCurrentPollsQuery, Result<IEnumerable<PollResponseDto>>>
{
    public async Task<Result<IEnumerable<PollResponseDto>>> Handle(GetCurrentPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetCurrentAsync(cancellationToken);
        return Result.Success(polls.Adapt<IEnumerable<PollResponseDto>>());
    }
}