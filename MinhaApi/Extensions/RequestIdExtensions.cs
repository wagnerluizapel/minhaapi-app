using Serilog.Context;

namespace MinhaApi.Extensions;

public static class RequestIdExtensions
{
    public static void UseRequestIdMiddleware(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;
            LogContext.PushProperty("RequestId", requestId);
            await next();
        });
    }
}
