using BoringTemplate.API.DAL.Entities;
using BoringTemplate.API.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BoringTemplate.API.DAL.Repositories;

public class WeatherForecastRepository(AppDbContext dbContext) : IWeatherForecastRepository
{
    public async Task<LocationForecastEntity?> GetForecast(string city)
    {
        return await dbContext.WeatherForecasts
            .Include(f => f.DailyForecast).FirstOrDefaultAsync(wf => wf.City.ToUpper() == city.ToUpper());
    }

    public async Task<string> CreateForecast(LocationForecastEntity forecastEntity)
    {
        var createdForecast = await dbContext.WeatherForecasts.AddAsync(forecastEntity);
        await dbContext.SaveChangesAsync();
        return forecastEntity.Id;
    }
}
