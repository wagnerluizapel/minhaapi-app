using System.Text.RegularExpressions;

public class InputSanitizationMiddleware
{
    private readonly RequestDelegate _next;

    public InputSanitizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sanitizedQuery = new Dictionary<string, string>();

        foreach (var key in context.Request.Query.Keys)
        {
            var value = context.Request.Query[key].ToString();
            sanitizedQuery[key] = Sanitize(value);
        }

        // Armazena a versão sanitizada para uso posterior
        context.Items["SanitizedQuery"] = sanitizedQuery;

        await _next(context);
    }

    private string Sanitize(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        input = Regex.Replace(input, "<.*?>", string.Empty);

        input = input.Replace(";", "")
                     .Replace("--", "")
                     .Replace("'", "")
                     .Replace("\"", "");

        return input;
    }
}
