using SurveyBasket.API.Common;
using SurveyBasket.Application.Common;

namespace SurveyBasket.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult(Result result, int? statusCode = default)
    {
        if (result.IsSuccess)
            return StatusCode(GetSuccessStatusCode(statusCode));

        return result.ToProblem(MapFailureStatusCode(result.Error.Code));
    }

    protected IActionResult HandleResult<T>(Result<T> result, int? statusCode = default)
    {
        if (result.IsSuccess)
            return StatusCode(GetSuccessStatusCode(statusCode), result.Value);

        return result.ToProblem(MapFailureStatusCode(result.Error.Code));
    }

    private static int MapFailureStatusCode(string failureCode)
    {
        var reason = failureCode.Split('.').LastOrDefault() ?? "";

        return reason switch
        {
            "Validation" => StatusCodes.Status400BadRequest,
            "Unauthorized" or "InvalidCredentials" or "InvalidJwtToken" or "InvalidRefreshToken"
                    => StatusCodes.Status401Unauthorized,
            "NotFound" => StatusCodes.Status404NotFound,
            "Conflict" => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private int GetSuccessStatusCode(int? statusCode)
    {
        if (statusCode.HasValue)
            return statusCode.Value;

        var method = HttpContext.Request.Method;

        return method switch
        {
            "POST" => StatusCodes.Status201Created,
            "PUT" => StatusCodes.Status204NoContent,
            "DELETE" => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status200OK
        };
    }
}