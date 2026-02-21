namespace SurveyBasket.Application.Polls.Commands.UpdatePoll;

public class UpdatePollCommandValidator : AbstractValidator<UpdatePollCommand>
{
    public UpdatePollCommandValidator()
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
            .WithName(nameof(UpdatePollCommand.EndsAt))
            .WithMessage("{PropertyName} must be greater than or equals start date");
    }

    private bool HasValidDates(UpdatePollCommand command)
    {
        return command.EndsAt >= command.StartsAt;
    }
}