// SurveyBasket.Application/Polls/Commands/CreatePoll/CreatePollValidator.cs
namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandValidator : AbstractValidator<CreatePollCommand>
{
    public CreatePollCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Summary)
            .NotEmpty()
            .Length(3, 1500);

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt)
            .NotEmpty();

        RuleFor(x => x)
            .Must(HasValidDates)
            .WithName(nameof(CreatePollCommand.EndsAt))
            .WithMessage("{PropertyName} must be greater than or equals start date");
    }

    private bool HasValidDates(CreatePollCommand command)
    {
        return command.EndsAt >= command.StartsAt;
    }
}