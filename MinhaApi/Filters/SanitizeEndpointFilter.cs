using System.Text.RegularExpressions;

public class SanitizeEndpointFilter : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        foreach (var arg in context.Arguments)
        {
            SanitizeObject(arg);
        }

        return await next(context);
    }

    private void SanitizeObject(object? obj)
    {
        if (obj == null)
            return;

        var props = obj.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string) && p.CanWrite);

        foreach (var prop in props)
        {
            var value = (string?)prop.GetValue(obj);
            if (value != null)
            {
                var sanitized = Regex.Replace(value, "<.*?>", string.Empty)
                                     .Replace(";", "")
                                     .Replace("--", "")
                                     .Replace("'", "");
                prop.SetValue(obj, sanitized);
            }
        }
    }
}
