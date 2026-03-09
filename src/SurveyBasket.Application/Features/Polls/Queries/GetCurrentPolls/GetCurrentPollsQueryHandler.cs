using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetCurrentPolls;

public class GetCurrentPollsQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetCurrentPollsQuery, Result<IEnumerable<PollDto>>>
{
    public async Task<Result<IEnumerable<PollDto>>> Handle(GetCurrentPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetCurrentAsync(cancellationToken);
        return Result.Success(polls.Adapt<IEnumerable<PollDto>>());
    }
}