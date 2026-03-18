using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetCurrentPollsV2;

public class GetCurrentPollsQueryHandlerV2(IPollRepository pollRepository)
    : IRequestHandler<GetCurrentPollsQueryV2, Result<IEnumerable<PollResponseV2>>>
{
    public async Task<Result<IEnumerable<PollResponseV2>>> Handle(GetCurrentPollsQueryV2 request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetCurrentAsync(cancellationToken);
        return Result.Success(polls.Adapt<IEnumerable<PollResponseV2>>());
    }
}