namespace SurveyBasket.Application.Common.Constants;

public static class CacheKeys
{
    public const string AllPolls = "polls:all";
    public const string CurrentPolls = "polls:current";

    public static string PollById(int id) => $"polls:{id}";
    public static string PollVotes(int id) => $"polls:{id}:votes";
    public static string QuestionsByPollId(int pollId, int pageNumber = 1, int pageSize = 10, string? searchValue = null, string? sortColumn = null, string? sortDirection = null)
        => $"polls:{pollId}:questions:page:{pageNumber}:size:{pageSize}:search:{searchValue ?? "*"}:sort:{sortColumn ?? "*"}:{sortDirection ?? "*"}";
    public static string QuestionsByPollIdPrefix(int pollId) => $"polls:{pollId}:questions";
    public static string QuestionById(int pollId, int questionId) => $"polls:{pollId}:questions:{questionId}";
    public static string VotesPerDay(int pollId) => $"polls:{pollId}:votes:perday";
    public static string VotesPerQuestion(int pollId) => $"polls:{pollId}:votes:perquestion";
    public static string AvailableQuestions(int pollId, string userId) => $"polls:{pollId}:available:{userId}";
}