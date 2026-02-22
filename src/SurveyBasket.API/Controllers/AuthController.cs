using Mapster;
using MediatR;
using SurveyBasket.API.Controllers.Base;
using SurveyBasket.Application.Authentication.Commands.Login;
using SurveyBasket.Application.Authentication.Commands.RefreshToken;
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
        return HandleResult(result);
    }

    [HttpPut("")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<RefreshTokenCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<RevokeRefreshTokenCommand>();
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}