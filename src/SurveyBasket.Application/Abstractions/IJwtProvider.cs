using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Abstractions;

public interface IJwtProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);
}