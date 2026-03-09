namespace SurveyBasket.Application.Common.Interfaces;

public interface IApplicationUrlService
{
    /// <summary>Returns the request Origin header, or null if not present.</summary>
    string? GetOrigin();
}
