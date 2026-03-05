using SurveyBasket.Infrastructure.Persistence;

namespace SurveyBasket.Infrastructure.Repositories;

public class ResultRepository(ApplicationDbContext context) : IResultRepository
{
    public async Task<Poll?> GetPollWithVotesAsync(int pollId, CancellationToken cancellationToken)
     => await context.Polls
        //.AsSplitQuery()
        .Include(p => p.Votes)
            .ThenInclude(v => v.User)
        .Include(p => p.Votes)
            .ThenInclude(v => v.VoteAnswers)
                .ThenInclude(va => va.Question)
        .Include(p => p.Votes)
            .ThenInclude(v => v.VoteAnswers)
                .ThenInclude(va => va.Answer)
        .FirstOrDefaultAsync(p => p.Id == pollId, cancellationToken);




    //public async Task<Poll?> GetPollWithVotesAsync(int pollId, CancellationToken cancellationToken = default)
    //{
    //    var connection = context.Database.GetDbConnection();

    //    using (var command = connection.CreateCommand())
    //    {
    //        command.CommandText = @"
    //        SELECT 
    //            p.Id, p.Title, p.Summary, p.IsPublished, p.StartsAt, p.EndsAt,
    //            v.Id as VoteId, v.SubmittedOn,
    //            u.Id as UserId, u.FirstName, u.LastName, u.Email,
    //            va.Id as VoteAnswerId,
    //            q.Id as QuestionId, q.Content as QuestionContent,
    //            a.Id as AnswerId, a.Content as AnswerContent
    //        FROM Polls p
    //        LEFT JOIN Votes v ON p.Id = v.PollId
    //        LEFT JOIN AspNetUsers u ON v.UserId = u.Id
    //        LEFT JOIN VoteAnswers va ON v.Id = va.VoteId
    //        LEFT JOIN Questions q ON va.QuestionId = q.Id
    //        LEFT JOIN Answers a ON va.AnswerId = a.Id
    //        WHERE p.Id = @pollId
    //        ORDER BY v.SubmittedOn DESC";

    //        var parameter = command.CreateParameter();
    //        parameter.ParameterName = "@pollId";
    //        parameter.Value = pollId;
    //        command.Parameters.Add(parameter);

    //        await connection.OpenAsync(cancellationToken);

    //        using (var reader = await command.ExecuteReaderAsync(cancellationToken))
    //        {
    //            Poll? poll = null;
    //            var votesDict = new Dictionary<int, Vote>();

    //            while (await reader.ReadAsync(cancellationToken))
    //            {
    //                if (poll is null)
    //                {
    //                    poll = new Poll
    //                    {
    //                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
    //                        Title = reader.GetString(reader.GetOrdinal("Title")),
    //                        Summary = reader.GetString(reader.GetOrdinal("Summary")),
    //                        IsPublished = reader.GetBoolean(reader.GetOrdinal("IsPublished")),
    //                        StartsAt = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("StartsAt"))),
    //                        EndsAt = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("EndsAt")))
    //                    };
    //                }

    //                if (!reader.IsDBNull(reader.GetOrdinal("VoteId")))
    //                {
    //                    var voteId = reader.GetInt32(reader.GetOrdinal("VoteId"));

    //                    if (!votesDict.TryGetValue(voteId, out var vote))
    //                    {
    //                        vote = new Vote
    //                        {
    //                            Id = voteId,
    //                            SubmittedOn = reader.GetDateTime(reader.GetOrdinal("SubmittedOn")),
    //                            UserId = reader.GetString(reader.GetOrdinal("UserId")),
    //                            User = new ApplicationUser
    //                            {
    //                                Id = reader.GetString(reader.GetOrdinal("UserId")),
    //                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
    //                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
    //                                Email = reader.GetString(reader.GetOrdinal("Email"))
    //                            }
    //                        };
    //                        votesDict[voteId] = vote;
    //                        poll.Votes.Add(vote);
    //                    }

    //                    if (!reader.IsDBNull(reader.GetOrdinal("VoteAnswerId")))
    //                    {
    //                        var voteAnswer = new VoteAnswer
    //                        {
    //                            Id = reader.GetInt32(reader.GetOrdinal("VoteAnswerId")),
    //                            VoteId = voteId,
    //                            QuestionId = reader.GetInt32(reader.GetOrdinal("QuestionId")),
    //                            AnswerId = reader.GetInt32(reader.GetOrdinal("AnswerId")),
    //                            Question = new Question
    //                            {
    //                                Id = reader.GetInt32(reader.GetOrdinal("QuestionId")),
    //                                Content = reader.GetString(reader.GetOrdinal("QuestionContent"))
    //                            },
    //                            Answer = new Answer
    //                            {
    //                                Id = reader.GetInt32(reader.GetOrdinal("AnswerId")),
    //                                Content = reader.GetString(reader.GetOrdinal("AnswerContent"))
    //                            }
    //                        };

    //                        vote.VoteAnswers.Add(voteAnswer);
    //                    }
    //                }
    //            }

    //            return poll;
    //        }
    //    }
    //}


    public async Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId, CancellationToken cancellationToken) =>
        await context.Votes
            .AsNoTracking()
            .Where(v => v.PollId == pollId)
            .Include(v => v.User)
            .Include(v => v.VoteAnswers)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<VoteAnswer>> GetVoteAnswersByPollIdAsync(int pollId, CancellationToken cancellationToken) =>
        await context.VoteAnswers
            .AsNoTracking()
            .Where(va => va.Vote.PollId == pollId)
            .Select(va => new VoteAnswer
            {
                Id = va.Id,
                QuestionId = va.QuestionId,
                AnswerId = va.AnswerId,
                VoteId = va.VoteId,
                Question = new Question
                {
                    Id = va.Question.Id,
                    Content = va.Question.Content,
                    IsActive = va.Question.IsActive
                },
                Answer = new Answer
                {
                    Id = va.Answer.Id,
                    Content = va.Answer.Content,
                    IsActive = va.Answer.IsActive
                }
            })
            .ToListAsync(cancellationToken);
}