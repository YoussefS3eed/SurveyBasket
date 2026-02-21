using Mapster;
using MediatR;
using SurveyBasket.API.Controllers.Base;
using SurveyBasket.Application.Authentication.Dtos;
using SurveyBasket.Application.Authentication.Queries.Login;

namespace SurveyBasket.API.Controllers;

[Route("[controller]")]
public class AuthController(ISender sender) : ApiController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        var query = request.Adapt<LoginQuery>();
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }
}