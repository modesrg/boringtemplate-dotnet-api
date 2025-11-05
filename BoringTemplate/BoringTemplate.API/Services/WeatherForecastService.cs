using BoringTemplate.API.DAL.Repositories.Interface;
using BoringTemplate.API.Mappings;
using BoringTemplate.API.Models;
using BoringTemplate.API.Services.Interfaces;

namespace BoringTemplate.API.Services;

public class WeatherForecastService(IWeatherForecastRepository weatherForecastRepository) : IWeatherForecastService
{
    public async Task<LocationForecast?> GetForecast(string city)
    {
        var forecastEntity = await weatherForecastRepository.GetForecast(city);
        return forecastEntity?.ToModel();
    }
    public async Task<string> CreateForecast(LocationForecast locationForecast)
    {
        var forecastEntity = locationForecast.ToEntity();
        return await weatherForecastRepository.CreateForecast(forecastEntity);
    }
}
