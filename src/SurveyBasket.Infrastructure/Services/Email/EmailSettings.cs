namespace SurveyBasket.Infrastructure.Services.Email;

public class EmailSettings
{
    public const string SectionName = "EmailSettings";

    public string Mail { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
}