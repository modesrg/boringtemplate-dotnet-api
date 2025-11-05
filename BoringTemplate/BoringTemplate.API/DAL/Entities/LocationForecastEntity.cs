namespace BoringTemplate.API.DAL.Entities;

public class LocationForecastEntity
{
    public required string Id { get; set; }

    public required string City { get; set; }

    public ICollection<DailyWeatherEntity> DailyForecast { get; set; } = new HashSet<DailyWeatherEntity>();
}
