namespace MinhaApi.DTOs.External;

public class WeatherResponse
{
    public string Cidade { get; set; }
    public double Temperatura { get; set; }
    public int Umidade { get; set; }
    public double Vento { get; set; }
    public string Descricao { get; set; }
}
