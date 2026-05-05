using MinhaApi.DTOs.External;

namespace MinhaApi.Services.External;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherAsync(string city);
}
