namespace MinhaApi.DTOs.External;

public class OpenWeatherApiResponse
{
    public MainInfo main { get; set; }
    public WindInfo wind { get; set; }
    public List<WeatherInfo> weather { get; set; }
}

public class MainInfo
{
    public double temp { get; set; }
    public int humidity { get; set; }
}

public class WindInfo
{
    public double speed { get; set; }
}

public class WeatherInfo
{
    public string description { get; set; }
}
