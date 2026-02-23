using SurveyBasket.Application.Errors;

namespace SurveyBasket.Application.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler(IPollRepository pollRepository)
    : IRequestHandler<GetPollByIdQuery, Result<PollDto>>
{
    public async Task<Result<PollDto>> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdAsync(request.Id, cancellationToken);

        if (poll is null)
            return Result.Failure<PollDto>(PollErrors.NotFound);

        return Result.Success(poll.Adapt<PollDto>());
    }
}