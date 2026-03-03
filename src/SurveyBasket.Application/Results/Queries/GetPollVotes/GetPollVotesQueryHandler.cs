using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetPollVotes;

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
            return Result.Failure<PollVotesResponse>(PollErrors.PollNotFound);

        var poll = await resultRepository.GetPollWithVotesAsync(request.PollId, cancellationToken);

        if (poll is null)
            return Result.Failure<PollVotesResponse>(PollErrors.PollNotFound);

        var response = poll.Adapt<PollVotesResponse>();
        return Result.Success(response);
    }
}