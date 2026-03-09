using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetPollByIdQuery, Result<PollResponse>>
{
    public async Task<Result<PollResponse>> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure<PollResponse>(PollErrors.NotFound(request.Id));

        return Result.Success(poll.Adapt<PollResponse>());
    }
}