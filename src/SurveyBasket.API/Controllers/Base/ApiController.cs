using SurveyBasket.Application.Common;

namespace SurveyBasket.API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok();

        return result.Error.Code switch
        {
            "Error.NotFound" => NotFound(new { error = result.Error.Description }),
            "Error.Validation" => BadRequest(new { error = result.Error.Description }),
            "Error.Unauthorized" => Unauthorized(new { error = result.Error.Description }),
            _ => StatusCode(500, new { error = result.Error.Description })
        };
    }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        return result.Error.Code switch
        {
            "Error.NotFound" => NotFound(new { error = result.Error.Description }),
            "Error.Validation" => BadRequest(new { error = result.Error.Description }),
            "Error.Unauthorized" => Unauthorized(new { error = result.Error.Description }),
            _ => StatusCode(500, new { error = result.Error.Description })
        };
    }
}