using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Errors;

namespace SurveyBasket.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result, string httpMethod, int? statusCode = default)
    {
        if (result.IsSuccess)
            return new StatusCodeResult(GetSuccessStatusCode(httpMethod, statusCode));

        return result.ToProblem();
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, string httpMethod, int? statusCode = default)
    {
        if (result.IsSuccess)
            return new ObjectResult(result.Value)
            {
                StatusCode = GetSuccessStatusCode(httpMethod, statusCode)
            };

        return result.ToProblem();
    }

    // Keep controller version if you want automatic method detection
    public static IActionResult ToActionResult(this Result result, ControllerBase controller, int? statusCode = default)
    {
        if (result.IsSuccess)
            return new StatusCodeResult(GetSuccessStatusCode(controller.Request.Method, statusCode));

        return result.ToProblem();
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller, int? statusCode = default)
    {
        if (result.IsSuccess)
            return new ObjectResult(result.Value)
            {
                StatusCode = GetSuccessStatusCode(controller.Request.Method, statusCode)
            };

        return result.ToProblem();
    }

    private static int GetSuccessStatusCode(string httpMethod, int? statusCode = null)
    {
        if (statusCode.HasValue)
            return statusCode.Value;

        return httpMethod.ToUpperInvariant() switch
        {
            "POST" => StatusCodes.Status201Created,
            "PUT" or "PATCH" or "DELETE" => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status200OK
        };
    }


    //private static ObjectResult ToProblem(this Result result)
    //{
    //    var problem = Results.Problem(statusCode: MapErrorTypeToStatusCode(result.Error.Type));
    //    var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

    //    problemDetails!.Extensions = new Dictionary<string, object?>
    //    {
    //        {
    //            "errors", new[]
    //            {
    //                result.Error.Code,
    //                result.Error.Description
    //            }
    //        }
    //    };

    //    return new ObjectResult(problemDetails);
    //}

    private static ObjectResult ToProblem(this Result result)
    {
        var statusCode = MapErrorTypeToStatusCode(result.Error.Type);

        var problemDetails = new ProblemDetails
        {
            Title = GetTitleForErrorType(result.Error.Type),
            Detail = result.Error.Description,
            Status = statusCode,
            Type = $"https://tools.ietf.org/html/rfc7231#section-6.5.{statusCode % 100}",
            Extensions = new Dictionary<string, object?>
            {
                { "errorCode", result.Error.Code }
            }
        };

        if (result.Error.Errors.Any())
        {
            problemDetails.Extensions["errors"] = result.Error.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }

        return new ObjectResult(problemDetails)
        {
            StatusCode = statusCode
        };
    }


    private static int MapErrorTypeToStatusCode(ErrorType errorType) => errorType switch
    {
        ErrorType.None => StatusCodes.Status200OK,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Failure => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };

    private static string GetTitleForErrorType(ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => "Bad Request",
        ErrorType.Unauthorized => "Unauthorized",
        ErrorType.Forbidden => "Forbidden",
        ErrorType.NotFound => "Not Found",
        ErrorType.Conflict => "Conflict",
        ErrorType.Failure => "Server Error",
        _ => "Error"
    };
}