// FILE: Domain/Errors/VoteErrors.cs
namespace SurveyBasket.Domain.Errors;

public static class VoteErrors
{
    public static readonly Error DuplicatedVote =
        Error.Conflict("Vote.Duplicated", "You have already voted in this poll.");

    public static readonly Error InvalidQuestions =
        Error.Validation("Vote.InvalidQuestions", "The submitted questions do not match the active poll questions.");

    public static Error PollNotOpen(object pollId) =>
        Error.Validation("Vote.PollNotOpen", $"Poll '{pollId}' is not currently open for voting.");
}