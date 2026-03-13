namespace SurveyBasket.Application.Features.Users.Commands.UpdateProfile;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);
    }
}
