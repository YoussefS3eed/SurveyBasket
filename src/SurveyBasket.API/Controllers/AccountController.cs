using SurveyBasket.Application.Features.Users.Commands.ChangePassword;
using SurveyBasket.Application.Features.Users.Commands.UpdateProfile;
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
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
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
