namespace SurveyBasket.Application.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote = new("Vote.Conflict", "This user already voted before for this poll", 409);
    public static readonly Error InvalidQuestions = new("Vote.Validation", "Invalid questions", 400);
}