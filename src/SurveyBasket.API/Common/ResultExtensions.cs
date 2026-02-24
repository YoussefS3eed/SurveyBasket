using SurveyBasket.Application.Common;

namespace SurveyBasket.API.Common;

public static class ResultExtensions
{
    public static ObjectResult ToProblem(this Result result, int statusCode)
    {
        var problem = Results.Problem(statusCode: statusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors", new[] { result.Error }
            }
        };

        return new ObjectResult(problemDetails);
    }

}