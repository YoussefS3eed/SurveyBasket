using System.Text;

namespace SurveyBasket.Application.Common.Extensions;

public static class StringExtensions
{
    public static string ToBase64UrlEncoded(this string value)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(value))
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');

    public static string FromBase64UrlEncoded(this string value)
    {
        var base64 = value
            .Replace('-', '+')
            .Replace('_', '/')
            + new string('=', (4 - value.Length % 4) % 4);

        return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
    }
}