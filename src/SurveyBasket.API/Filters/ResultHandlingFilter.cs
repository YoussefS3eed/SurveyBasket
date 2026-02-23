using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SurveyBasket.API.Common;
using SurveyBasket.Application.Common;
using SurveyBasket.Application.Errors;
using System.Reflection;

namespace SurveyBasket.API.Filters;

public class ResultHandlingFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is Result result)
        {
            context.Result = ConvertToActionResult(result, context);
        }
    }

    private IActionResult ConvertToActionResult(Result result, ActionExecutedContext context)
    {
        return result.IsSuccess
            ? HandleSuccess(result, context)
            : HandleFailure(result.Error, context);
    }

    private IActionResult HandleSuccess(Result result, ActionExecutedContext context)
    {
        var httpMethod = context.HttpContext.Request.Method;
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        // Special handling for POST with [CreatedAtAction] attribute
        if (httpMethod == "POST" && result.GetType().IsGenericType)
        {
            //var createdAtAttr = actionDescriptor?.MethodInfo.GetCustomAttribute<CreatedAtActionAttribute>();
            //if (createdAtAttr != null)
            //{
            //    var value = ExtractValue(result);
            //    if (value != null)
            //    {
            //        var id = ExtractId(value, createdAtAttr.IdProperty);
            //        if (id != null)
            //        {
            //            return new CreatedAtActionResult(
            //                createdAtAttr.ActionName,
            //                actionDescriptor.ControllerName,
            //                new { id },
            //                value
            //            );
            //        }
            //    }
            //}
        }

        // Default status codes based on HTTP method
        int statusCode = httpMethod switch
        {
            "POST" => StatusCodes.Status201Created,
            "DELETE" => StatusCodes.Status204NoContent,
            "PUT" => StatusCodes.Status204NoContent,
            _ => StatusCodes.Status200OK
        };

        if (result.GetType().IsGenericType)
        {
            var value = ExtractValue(result);
            return new ObjectResult(value) { StatusCode = statusCode };
        }
        else
        {
            return new StatusCodeResult(statusCode);
        }
    }

    private IActionResult HandleFailure(Error error, ActionExecutedContext context)
    {
        // Use your existing ResultExtensions.ToProblem to format the error response
        var failureResult = Result.Failure(error);
        int statusCode = MapErrorToStatusCode(error.Code);
        return failureResult.ToProblem(statusCode);
    }

    private static object? ExtractValue(Result result)
    {
        return result.GetType().GetProperty("Value")?.GetValue(result);
    }

    private static object? ExtractId(object value, string idPropertyName)
    {
        return value.GetType().GetProperty(idPropertyName)?.GetValue(value);
    }

    private static int MapErrorToStatusCode(string errorCode)
    {
        var reason = errorCode.Split('.').LastOrDefault() ?? "";
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
}