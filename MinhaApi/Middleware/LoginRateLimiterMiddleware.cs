using System.Collections.Concurrent;

namespace MinhaApi.Middleware;


public class LoginRateLimiterMiddleware
{
    private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> Attempts 
        = new();

    private readonly RequestDelegate _next;
    private const int LIMIT = 5; // 5 tentativas
    private static readonly TimeSpan WINDOW = TimeSpan.FromMinutes(1);

    public LoginRateLimiterMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/auth/login"))
        {
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var now = DateTime.UtcNow;

            var entry = Attempts.GetOrAdd(ip, _ => (0, now));

            if (now - entry.WindowStart > WINDOW)
            {
                Attempts[ip] = (1, now);
            }
            else
            {
                if (entry.Count >= LIMIT)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many login attempts. Try again later.");
                    return;
                }

                Attempts[ip] = (entry.Count + 1, entry.WindowStart);
            }
        }

        await _next(context);
    }
}
