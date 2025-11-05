namespace BoringTemplate.API.Models;

public class LocationForecast
{
    public string? Id { get; set; }

    public required string City { get; set; }

    public List<DailyWeather> DailyForecast { get; set; } = new();
}
