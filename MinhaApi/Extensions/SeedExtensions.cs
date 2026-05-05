using Microsoft.EntityFrameworkCore;
using MinhaApi.Data;
using MinhaApi.Models.Entities;

namespace MinhaApi.Extensions;

public static class SeedExtensions
{
    public static void ApplyMigrationsAndSeed(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // -----------------------------
        // MIGRATIONS COM RETRY
        // -----------------------------
        var maxRetries = 10;
        var delay = TimeSpan.FromSeconds(3);

        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                db.Database.Migrate();
                Console.WriteLine("Migrations aplicadas com sucesso!");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Tentativa {i + 1}/{maxRetries} falhou: {ex.Message}");

                if (i == maxRetries - 1)
                    throw;

                Thread.Sleep(delay);
            }
        }

        // -----------------------------
        // SEED DO USUÁRIO ADMIN
        // -----------------------------
        if (!db.Usuarios.Any())
        {
            db.Usuarios.Add(new Usuario
            {
                Nome = "Admin",
                Email = "admin@teste.com",
                SenhaHash = PasswordHasher.Hash("123456"),
                Role = Role.Admin
            });

            db.SaveChanges();
        }
    }
}
