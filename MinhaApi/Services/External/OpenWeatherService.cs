using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using MinhaApi.DTOs.External;
using MinhaApi.Services.External;

namespace MinhaApi.Services.External;

public class OpenWeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenWeatherService> _logger;
    private readonly string _apiKey;

    public OpenWeatherService(HttpClient httpClient, ILogger<OpenWeatherService> logger, IConfiguration config)
    {
        _httpClient = httpClient;
        _logger = logger;
        _apiKey = config["OPENWEATHER_API_KEY"] 
                  ?? throw new Exception("OPENWEATHER_API_KEY não configurada.");

        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<WeatherResponse?> GetWeatherAsync(string cidade)
    {
        try
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={_apiKey}&units=metric&lang=pt_br";

            _logger.LogInformation("Chamando OpenWeather para cidade {Cidade}", cidade);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Erro ao chamar OpenWeather. Status: {Status}", response.StatusCode);
                return null;
            }

            //novo dto formetemente tipado
            var json = await response.Content.ReadFromJsonAsync<OpenWeatherApiResponse>();

            if (json == null)
            {
                _logger.LogWarning("Falha ao deserializar resposta do OpenWeather");
                return null;
            }

            return new WeatherResponse
            {
                Cidade = cidade,
                Temperatura = json.main.temp,
                Umidade = json.main.humidity,
                Vento = json.wind.speed,
                Descricao = json.weather.First().description
            };

        }
        catch (TaskCanceledException)
        {
            _logger.LogError("Timeout ao chamar OpenWeather para cidade {Cidade}", cidade);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao chamar OpenWeather");
            return null;
        }
    }
}
