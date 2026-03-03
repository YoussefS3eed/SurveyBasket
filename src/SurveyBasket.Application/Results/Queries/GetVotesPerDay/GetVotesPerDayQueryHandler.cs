using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetVotesPerDay;

public class GetVotesPerDayQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetVotesPerDayQuery, Result<IEnumerable<VotesPerDayResponse>>>
{
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> Handle(
        GetVotesPerDayQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.PollNotFound);

        var votes = await resultRepository.GetVotesByPollIdAsync(request.PollId, cancellationToken);

        var votesPerDay = votes
            .GroupBy(v => DateOnly.FromDateTime(v.SubmittedOn))
            .Select(g => new VotesPerDayResponse(g.Key, g.Count()))
            .ToList();

        return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
    }
}