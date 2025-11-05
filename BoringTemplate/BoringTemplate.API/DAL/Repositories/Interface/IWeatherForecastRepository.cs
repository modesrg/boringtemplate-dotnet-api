using BoringTemplate.API.DAL.Entities;

namespace BoringTemplate.API.DAL.Repositories.Interface;

public interface IWeatherForecastRepository
{
    Task<LocationForecastEntity?> GetForecast(string city);
    Task<string> CreateForecast(LocationForecastEntity forecastEntity);
}