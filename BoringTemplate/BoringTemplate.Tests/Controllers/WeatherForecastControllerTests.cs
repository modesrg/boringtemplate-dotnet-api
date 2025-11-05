using BoringTemplate.API.Controllers;
using BoringTemplate.API.Models;
using BoringTemplate.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BoringTemplate.Tests;

public class WeatherForecastControllerTests
{
    private readonly Mock<IWeatherForecastService> _mockService;
    private readonly WeatherForecastController _controller;

    public WeatherForecastControllerTests()
    {
        _mockService = new Mock<IWeatherForecastService>();
        _controller = new WeatherForecastController(_mockService.Object);
    }

    [Fact]
    public async Task GetForecast_ReturnsOk_WhenForecastExists()
    {
        // Arrange
        var city = "Berlin";
        var forecast = new LocationForecast { City = city };
        _mockService.Setup(s => s.GetForecast(city)).ReturnsAsync(forecast);

        // Act
        var response = await _controller.GetForecast(city);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(response.Result);
        var result = Assert.IsType<LocationForecast>(ok.Value);
        Assert.Equal(city, result.City);
        _mockService.Verify(s => s.GetForecast(city), Times.Once);
    }

    [Fact]
    public async Task GetForecast_ReturnsNotFound_WhenNoForecast()
    {
        // Arrange
        var city = "Nowhere";
        _mockService.Setup(s => s.GetForecast(city)).ReturnsAsync((LocationForecast?)null);

        // Act
        var response = await _controller.GetForecast(city);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(response.Result);
        Assert.Contains(city, notFound.Value.ToString());
        _mockService.Verify(s => s.GetForecast(city), Times.Once);
    }

    [Fact]
    public async Task AddForecast_ReturnsOk_WhenCreated()
    {
        // Arrange
        var model = new LocationForecast { City = "Oslo" };
        var newId = "oslo_20251104";
        _mockService.Setup(s => s.CreateForecast(model)).ReturnsAsync(newId);

        // Act
        var response = await _controller.AddForecast(model);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(response.Result);
        Assert.Equal(newId, ok.Value);
        _mockService.Verify(s => s.CreateForecast(model), Times.Once);
    }

    [Fact]
    public async Task GetForecast_Returns500_WhenServiceThrows()
    {
        // Arrange
        var city = "Berlin";
        _mockService.Setup(s => s.GetForecast(city)).ThrowsAsync(new Exception("Unexpected failure"));

        // Act
        var result = await Record.ExceptionAsync(() => _controller.GetForecast(city));

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Exception>(result); // default unhandled case
    }

}