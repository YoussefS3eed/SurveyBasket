using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Application.Features.Users.Commands.ChangePassword;
using SurveyBasket.Application.Features.Users.Commands.ResendProfileVerificationCode;
using SurveyBasket.Application.Features.Users.Commands.UpdateProfile;
using SurveyBasket.Application.Features.Users.Commands.VerifyProfileEmail;
using SurveyBasket.Application.Features.Users.Queries.GetProfile;
using SurveyBasket.Domain.Common.Dtos;

namespace SurveyBasket.API.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public sealed class AccountController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Info(CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetProfileQuery(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("info")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("verify-email")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyProfileEmailCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("resend-verification-code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResendVerificationCode(CancellationToken cancellationToken)
    {
        return (await sender.Send(new ResendProfileVerificationCodeCommand(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this);
    }
}
