using BoringTemplate.API.Models;

namespace BoringTemplate.API.Services.Interfaces;

public interface IWeatherForecastService
{
    Task<string> CreateForecast(LocationForecast locationForecast);
    Task<LocationForecast?> GetForecast(string city);
}