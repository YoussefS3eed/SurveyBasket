namespace SurveyBasket.Application.Abstractions.Caching;

public static class ConstCacheKeys
{
    // Polls
    public const string AllPolls = "polls:all";
    public static string PollById(int id) => $"polls:{id}";
    public static string PollWithVotes(int id) => $"polls:{id}:votes";

    // Questions
    public static string QuestionsByPollId(int pollId) => $"polls:{pollId}:questions";
    public static string QuestionById(int pollId, int questionId) => $"polls:{pollId}:questions:{questionId}";

    // Results/Statistics
    public static string PollVotes(int pollId) => $"polls:{pollId}:votes:raw";
    public static string VotesPerDay(int pollId) => $"polls:{pollId}:votes:perday";
    public static string VotesPerQuestion(int pollId) => $"polls:{pollId}:votes:perquestion";

    // Available questions for voting
    public static string AvailableQuestions(int pollId, string userId) => $"polls:{pollId}:available:{userId}";
}