using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Results.Queries.GetPollVotes;

public class GetPollVotesQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetPollVotesQuery, Result<PollVotesResponseDto>>
{
    public async Task<Result<PollVotesResponseDto>> Handle(
        GetPollVotesQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<PollVotesResponseDto>(PollErrors.NotFound());

        var poll = await resultRepository.GetPollWithVotesAsync(request.PollId, cancellationToken);

        if (poll is null)
            return Result.Failure<PollVotesResponseDto>(PollErrors.NotFound());

        var response = poll.Adapt<PollVotesResponseDto>();
        return Result.Success(response);
    }
}