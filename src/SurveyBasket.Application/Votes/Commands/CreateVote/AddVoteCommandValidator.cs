using SurveyBasket.Application.Votes.Dtos;

namespace SurveyBasket.Application.Votes.Commands.CreateVote;

public class AddVoteCommandValidator : AbstractValidator<AddVoteCommand>
{
    public AddVoteCommandValidator()
    {
        RuleFor(x => x.Request.Answers)
            .NotEmpty();

        RuleForEach(x => x.Request.Answers)
            .SetValidator(new VoteAnswerRequestValidator());
    }
}

public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(x => x.QuestionId)
            .GreaterThan(0);

        RuleFor(x => x.AnswerId)
            .GreaterThan(0);
    }
}