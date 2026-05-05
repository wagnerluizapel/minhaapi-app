using Microsoft.EntityFrameworkCore;
using Serilog;
using MinhaApi.Data;

namespace MinhaApi.Extensions;

public static class DatabaseExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                config.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
            );

            // Logs detalhados do EF Core
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();

            // Log de comandos SQL
            options.LogTo(
                Log.Information,
                new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information
            );
        });
    }
}
