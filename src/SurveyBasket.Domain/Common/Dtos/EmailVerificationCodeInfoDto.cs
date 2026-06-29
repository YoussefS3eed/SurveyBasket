namespace SurveyBasket.Domain.Common.Dtos;

public record EmailVerificationCodeInfoDto(
    string Code,
    string NewEmail,
    DateTime ExpiresAt,
    bool IsUsed
)
{
    public bool IsExpired => ExpiresAt < DateTime.UtcNow;
}
