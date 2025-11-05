using BoringTemplate.API.DAL.Entities;
using BoringTemplate.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BoringTemplate.API.Demo;

public static class DbSeeder
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly string[] Cities = { "Oslo", "Berlin", "Tokyo", "New York", "Sydney", "Madrid" };

    public static async Task SeedIfEmptyAsync(AppDbContext context)
    {
        if (await context.WeatherForecasts.AnyAsync())
            return;

        var forecasts = Cities.Select(city => new LocationForecastEntity
        {
            Id = IdGenerator.Generate(),
            City = city,
            DailyForecast = Enumerable.Range(1, 5).Select(index => new DailyWeatherEntity
            {
                Id = IdGenerator.Generate(),
                //Id = city.Replace(" ", "").ToLower() + "_" + DateOnly.FromDateTime(DateTime.Now.AddDays(index)).ToString("yyyyMMdd"),
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList()
        }).ToList();

        await context.WeatherForecasts.AddRangeAsync(forecasts);
        await context.SaveChangesAsync();
    }
}
