using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinhaApi.Data;
using MinhaApi.Models.Dtos;
using MinhaApi.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinhaApi.Extensions;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app, IConfiguration config)
    {
        app.MapPost("/auth/login", async (
            LoginRequest request,
            AppDbContext db,
            IValidator<LoginRequest> validator) =>
        {
            // 1) DTO validation
            var validation = request.ValidateRequest(validator);
            if (validation != null)
                return validation;

            // 2) User lookup
            var usuario = await db.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Results.Unauthorized();

            // 3) Password check
            if (!PasswordHasher.Check(usuario.SenhaHash, request.Senha))
                return Results.Unauthorized();

            // 4) Claims
            var claims = new[]
            {
                new Claim("email", usuario.Email),
                new Claim("role", usuario.Role.ToString())
            };

            // 5) JWT
            var secret = config["JwtSecret"]
                ?? throw new Exception("JwtSecret not configured!");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new { token = tokenString });
        })
        .AllowAnonymous()
        .WithTags("Authentication")
        .WithSummary("Performs login in the API")
        .WithDescription("Validates the provided credentials and returns a JWT token valid for 1 hour.")
        .Accepts<LoginRequest>("application/json")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .RequireRateLimiting("LoginPolicy")
        .WithOpenApi(op =>
        {
            // Example request
            op.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["email"] = new OpenApiString("user@example.com"),
                            ["senha"] = new OpenApiString("123456")
                        }
                    }
                }
            };

            // Example response
            op.Responses["200"] = new OpenApiResponse
            {
                Description = "Login successfully completed",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["token"] = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")
                        }
                    }
                }
            };

            return op;
        });
    }
}
