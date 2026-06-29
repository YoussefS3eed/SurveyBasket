using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetAllPolls;

public class GetAllPollsQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetAllPollsQuery, Result<IEnumerable<PollResponseDto>>>
{
    public async Task<Result<IEnumerable<PollResponseDto>>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetAllAsync(cancellationToken);
        return Result.Success(polls.Adapt<IEnumerable<PollResponseDto>>());
    }
}