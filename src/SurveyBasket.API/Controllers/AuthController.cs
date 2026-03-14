using SurveyBasket.Application.Features.Authentication.Commands.ConfirmEmail;
using SurveyBasket.Application.Features.Authentication.Commands.ForgetPassword;
using SurveyBasket.Application.Features.Authentication.Commands.Login;
using SurveyBasket.Application.Features.Authentication.Commands.RefreshToken;
using SurveyBasket.Application.Features.Authentication.Commands.Register;
using SurveyBasket.Application.Features.Authentication.Commands.ResendConfirmationEmail;
using SurveyBasket.Application.Features.Authentication.Commands.ResetPassword;
using SurveyBasket.Application.Features.Authentication.Commands.RevokeRefreshToken;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }

    [HttpPut("refresh-token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 204);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }

    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationEmailCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }

    [HttpPost("forget-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }
}