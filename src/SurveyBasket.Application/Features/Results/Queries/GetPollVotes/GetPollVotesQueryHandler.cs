using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Results.Queries.GetPollVotes;

public class GetPollVotesQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetPollVotesQuery, Result<PollVotesResponse>>
{
    public async Task<Result<PollVotesResponse>> Handle(
        GetPollVotesQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<PollVotesResponse>(PollErrors.NotFound());

        var poll = await resultRepository.GetPollWithVotesAsync(request.PollId, cancellationToken);

        if (poll is null)
            return Result.Failure<PollVotesResponse>(PollErrors.NotFound());

        var response = poll.Adapt<PollVotesResponse>();
        return Result.Success(response);
    }
}