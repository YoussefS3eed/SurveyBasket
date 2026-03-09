using System.Reflection;

namespace SurveyBasket.Infrastructure.Services.Email;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string templateName, Dictionary<string, string> templateModel)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var templatePath = Path.Combine(assemblyLocation, "Services", "Email", "Templates", $"{templateName}.html");

        if (!File.Exists(templatePath))
            throw new FileNotFoundException($"Email template '{templateName}' not found at {templatePath}");

        var body = File.ReadAllText(templatePath);

        foreach (var item in templateModel)
            body = body.Replace(item.Key, item.Value);

        return body;
    }
}