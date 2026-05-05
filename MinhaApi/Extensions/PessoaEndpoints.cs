using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinhaApi.Models.Dtos;
using MinhaApi.Services;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MinhaApi.Extensions;

public static class PessoaEndpoints
{
    public static void MapPessoaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/pessoas")
                       .RequireAuthorization()
                       .WithTags("People")
                       .WithGroupName("People");

        // GET /pessoas
        group.MapGet("/", (PessoaService service) =>
        {
            return service.Listar();
        })
        .WithSummary("Lists all registered people")
        .WithDescription("Returns a list containing all people stored in the database.")
        .Produces<List<PessoaResponseDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        // GET /pessoas/{nome}
        group.MapGet("/{nome}", (string nome, PessoaService service, ILogger<Program> logger) =>
        {
            logger.LogInformation("Received GET /pessoas/{nome} request", nome);

            var result = service.Buscar(nome);

            if (!result.Success)
                logger.LogWarning("Person not found: {Name}", nome);

            return result;
        })
        .WithSummary("Searches for a person by name")
        .WithDescription("Returns the data of a specific person based on the provided name.")
        .Produces<PessoaResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized);

        // POST /pessoas
        group.MapPost("/", async (
            PessoaCreateDto dto,
            PessoaService service,
            IValidator<PessoaCreateDto> validator) =>
        {
            var validation = dto.ValidateRequest(validator);
            if (validation != null)
                return validation;

            return Results.Ok(service.Criar(dto));
        })
        .WithSummary("Creates a new person")
        .WithDescription("Receives the data of a new person and registers it in the database.")
        .Accepts<PessoaCreateDto>("application/json")
        .Produces<PessoaResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .AddEndpointFilter<SanitizeEndpointFilter>()
        .WithOpenApi(op =>
        {
            // Example of valid payload
            op.RequestBody = new OpenApiRequestBody
            {
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["nome"] = new OpenApiString("Wagner"),
                            ["idade"] = new OpenApiDouble(30)
                        }
                    }
                }
            };

            return op;
        });

        // PUT /pessoas/{nome}
        group.MapPut("/{nome}", async (
            string nome,
            PessoaUpdateDto dto,
            PessoaService service,
            IValidator<PessoaUpdateDto> validator) =>
        {
            var validation = dto.ValidateRequest(validator);
            if (validation != null)
                return validation;

            return Results.Ok(service.Atualizar(nome, dto));
        })
        .WithSummary("Updates a person's data")
        .WithDescription("Updates the data of an existing person based on the provided name.")
        .Accepts<PessoaUpdateDto>("application/json")
        .Produces<PessoaResponseDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .AddEndpointFilter<SanitizeEndpointFilter>();

        // DELETE /pessoas/{nome}
        group.MapDelete("/{nome}", (string nome, PessoaService service) =>
        {
            return service.Remover(nome);
        })
        .WithSummary("Deletes a person by name")
        .WithDescription("Removes a person from the database based on the provided name.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized);
    }
}
