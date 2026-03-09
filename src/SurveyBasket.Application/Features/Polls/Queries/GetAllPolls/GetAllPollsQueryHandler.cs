using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetAllPolls;

public class GetAllPollsQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetAllPollsQuery, Result<IEnumerable<PollDto>>>
{
    public async Task<Result<IEnumerable<PollDto>>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetAllAsync(cancellationToken);
        return Result.Success(polls.Adapt<IEnumerable<PollDto>>());
    }
}