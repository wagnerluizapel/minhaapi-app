using MinhaApi.Services.External;
using MinhaApi.DTOs.External;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinhaApi.Extensions;

public static class ExternalEndpoints
{
    public static void MapExternalEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/externo")
                       .WithTags("External Integration")
                       .WithGroupName("External Integration")
                       .RequireAuthorization();

        group.MapGet("/clima/{cidade}", async (
            string cidade,
            OpenWeatherService weatherService) =>
        {
            var resultado = await weatherService.GetWeatherAsync(cidade);

            if (resultado is null)
            {
                return Results.NotFound(new
                {
                    success = false,
                    error = "Unable to retrieve weather information for the specified city."
                });
            }

            return Results.Ok(new
            {
                success = true,
                data = resultado
            });
        })
        .WithSummary("Gets the current weather for a city")
        .WithDescription("Calls the external OpenWeather API and returns updated weather information for the specified city.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .WithOpenApi(op =>
        {
            // Success response example
            op.Responses["200"] = new OpenApiResponse
            {
                Description = "Weather retrieved successfully",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["success"] = new OpenApiBoolean(true),
                            ["data"] = new OpenApiObject
                            {
                                ["temperatura"] = new OpenApiDouble(28.5),
                                ["descricao"] = new OpenApiString("clear sky"),
                                ["umidade"] = new OpenApiDouble(60),
                                ["vento"] = new OpenApiDouble(3.4)
                            }
                        }
                    }
                }
            };

            // Error response example
            op.Responses["404"] = new OpenApiResponse
            {
                Description = "City not found",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["success"] = new OpenApiBoolean(false),
                            ["error"] = new OpenApiString("Unable to retrieve weather information for the specified city.")
                        }
                    }
                }
            };

            return op;
        });
    }
}
