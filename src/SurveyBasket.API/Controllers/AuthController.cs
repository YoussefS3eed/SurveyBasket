using SurveyBasket.Application.Authentication.Commands.ConfirmEmail;
using SurveyBasket.Application.Authentication.Commands.Login;
using SurveyBasket.Application.Authentication.Commands.RefreshToken;
using SurveyBasket.Application.Authentication.Commands.Register;
using SurveyBasket.Application.Authentication.Commands.ResendConfirmationEmail;
using SurveyBasket.Application.Authentication.Commands.RevokeRefreshToken;
using SurveyBasket.Application.Authentication.Dtos;

namespace SurveyBasket.API.Controllers;


[Route("[controller]")]
public class AuthController(ISender sender) : ApiController
{
    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<LoginCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, 200);
    }

    [HttpPut("")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<RefreshTokenCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, 200);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<RevokeRefreshTokenCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, 204);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<RegisterCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<ConfirmEmailCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, 200);
    }

    [HttpPost("resend-confirmation")]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendConfirmationEmailRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<ResendConfirmationEmailCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result, 200);
    }
}