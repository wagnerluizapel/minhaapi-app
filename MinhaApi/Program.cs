using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinhaApi.Data;
using MinhaApi.Extensions;
using MinhaApi.Repositories;
using MinhaApi.Services;
using MinhaApi.Services.External;
using MinhaApi.Validators;
using Serilog;
using System.Threading.RateLimiting;
using MinhaApi.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Kestrel listening on Docker
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Logging
builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, config) =>
{
    config
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithProcessId()
        .Enrich.WithThreadId()
        .WriteTo.Console(outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}");
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "People Management API",
        Version = "v1",
        Description = "Minimal API built with .NET 8 focused on security, clean architecture, observability, and production-ready practices.",
        Contact = new OpenApiContact { Name = "Wagner Luiz Apel", Email = "your-email@example.com" },
        License = new OpenApiLicense { Name = "MIT License" }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insert your JWT token like this: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.TagActionsBy(api => new[] { api.GroupName ?? "Others" });
    options.DocInclusionPredicate((name, api) => true);
});

// Services
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<AuditService>();
builder.Services.AddValidatorsFromAssemblyContaining<PessoaCreateValidator>();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHttpClient<OpenWeatherService>();
// Weather
builder.Services.AddScoped<IWeatherService, OpenWeatherService>();
builder.Services.AddHttpClient<OpenWeatherService>();


// Forwarded Headers
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;

    options.AddPolicy("LoginPolicy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: "login",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            }));
});

// CORS
var allowedOrigins = new[]
{
    "http://localhost:3000",
    "https://seu-dominio.com"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "People Management API — Documentation";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
});

// DB migrations + seed
app.ApplyMigrationsAndSeed();

// Middlewares
app.UseForwardedHeaders();
app.UseSerilogRequestLogging();
app.UseRequestIdMiddleware();
app.UseMiddleware<AuditMiddleware>();

app.UseRateLimiter();
app.UseCors("CorsPolicy");
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseMiddleware<InputSanitizationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthEndpoints();

app.MapPessoaEndpoints();
app.MapAuthEndpoints(builder.Configuration);
app.MapExternalEndpoints();

app.Run();

