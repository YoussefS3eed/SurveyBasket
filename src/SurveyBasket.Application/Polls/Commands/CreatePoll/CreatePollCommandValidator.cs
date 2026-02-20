namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandValidator : AbstractValidator<CreatePollCommand>
{
    public CreatePollCommandValidator()
    {
        RuleFor(x => x.PollRequestDto.Title)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.PollRequestDto.Summary)
            .NotEmpty()
            .Length(3, 1500);


        RuleFor(x => x.PollRequestDto.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.PollRequestDto.EndsAt)
            .NotEmpty();

        RuleFor(x => x.PollRequestDto.EndsAt)
            .GreaterThanOrEqualTo(x => x.PollRequestDto.StartsAt)
            .WithMessage("End date must be after start date");
    }
}
