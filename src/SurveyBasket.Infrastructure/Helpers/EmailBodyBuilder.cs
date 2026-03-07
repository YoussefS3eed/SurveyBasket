namespace SurveyBasket.Infrastructure.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string templateName, Dictionary<string, string> templateModel)
    {
        var templatePath = $"{AppContext.BaseDirectory}/Templates/{templateName}.html";

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Email template '{templateName}' not found at {templatePath}");

        var body = File.ReadAllText(templatePath);

        foreach (var item in templateModel)
            body = body.Replace(item.Key, item.Value);

        return body;
    }
}