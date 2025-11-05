using BoringTemplate.API.DAL.Entities;
using BoringTemplate.API.Helpers;
using BoringTemplate.API.Models;

namespace BoringTemplate.API.Mappings;

public static class ForecastMappings
{
    public static LocationForecast? ToModel(this LocationForecastEntity entity)
    {
        if (entity == null) return null;

        return new LocationForecast
        {
            Id = entity.Id,
            City = entity.City,
            DailyForecast = entity.DailyForecast.Select(d => new DailyWeather
            {
                Id = d.Id,
                Date = d.Date,
                TemperatureC = d.TemperatureC,
                Summary = d.Summary
            }).ToList()
        };
    }

    public static LocationForecastEntity ToEntity(this LocationForecast model)
    {
        return new LocationForecastEntity
        {
            Id = model.Id ?? IdGenerator.Generate(),
            City = model.City,
            DailyForecast = model.DailyForecast.Select(d => new DailyWeatherEntity
            {
                Id = d.Id,
                Date = d.Date,
                TemperatureC = d.TemperatureC,
                Summary = d.Summary
            }).ToList()
        };
    }
}



