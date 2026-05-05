using MinhaApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MinhaApi.Extensions;

public static class HealthExtensions
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", () => Results.Ok("Healthy"));

        app.MapGet("/ready", async (AppDbContext db) =>
        {
            try
            {
                await db.Database.CanConnectAsync();
                return Results.Ok("Ready");
            }
            catch
            {
                return Results.StatusCode(503);
            }
        });
    }
}
