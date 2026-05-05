namespace MinhaApi.Middleware;

public class AuditMiddleware
{
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AuditService audit)
    {
        if (context.Request.Method is "POST" or "PUT" or "DELETE")
        {
            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            var usuario = context.User?.FindFirst("email")?.Value ?? "anonymous";
            var endpoint = context.Request.Path;
            var metodo = context.Request.Method;
            var requestId = context.TraceIdentifier;

            await audit.RegistrarAsync(usuario, metodo, endpoint, body, requestId);
        }

        await _next(context);
    }
}

