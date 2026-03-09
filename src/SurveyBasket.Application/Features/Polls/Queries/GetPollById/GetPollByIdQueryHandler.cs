using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetPollByIdQuery, Result<PollDto>>
{
    public async Task<Result<PollDto>> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure<PollDto>(PollErrors.NotFound(request.Id));

        return Result.Success(poll.Adapt<PollDto>());
    }
}