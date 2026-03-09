using SurveyBasket.Application.Features.Votes.Dtos;

namespace SurveyBasket.Application.Features.Votes.Commands.CreateVote;

public class AddVoteCommandValidator : AbstractValidator<AddVoteCommand>
{
    public AddVoteCommandValidator()
    {
        RuleFor(x => x.Answers)
            .NotEmpty();

        RuleForEach(x => x.Answers)
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