using Microsoft.AspNetCore.Http;
using SurveyBasket.Application.Common.Interfaces;

namespace SurveyBasket.Infrastructure.Services;

internal sealed class ApplicationUrlService(IHttpContextAccessor httpContextAccessor)
    : IApplicationUrlService
{
    public string? GetOrigin() =>
        httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();
}
