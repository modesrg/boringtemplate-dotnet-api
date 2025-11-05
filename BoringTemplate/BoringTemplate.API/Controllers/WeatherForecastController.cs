using BoringTemplate.API.Models;
using BoringTemplate.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoringTemplate.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController(IWeatherForecastService weatherForecastService) : ControllerBase
{

    [HttpGet("get")]
    public async Task<ActionResult<LocationForecast>> GetForecast(string city = "New York")
    {
        var result = await weatherForecastService.GetForecast(city);
        return result is not null ? Ok(result) : NotFound($"Forecast for {city} not found");
    }

    [HttpPost("create")]
    public async Task<ActionResult<LocationForecast>> AddForecast(LocationForecast locationForecast)
    {
        var result = await weatherForecastService.CreateForecast(locationForecast);
        return result is not null ? Ok(result) : NotFound($"Forecast for {locationForecast.City} not found");
    }
}
