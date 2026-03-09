namespace SurveyBasket.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? Id { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}
