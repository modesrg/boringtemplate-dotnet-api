using BoringTemplate.API.DAL.Entities;
using BoringTemplate.API.DAL.Repositories.Interface;
using BoringTemplate.API.Mappings;
using BoringTemplate.API.Models;
using BoringTemplate.API.Services;
using Moq;

namespace BoringTemplate.Tests.Services;

public class WeatherForecastServiceTests
{
    private readonly Mock<IWeatherForecastRepository> _mockRepo = new();
    private readonly WeatherForecastService _service;

    public WeatherForecastServiceTests()
    {
        _service = new WeatherForecastService(_mockRepo.Object);
    }

    [Fact]
    public async Task GetForecast_ReturnsMappedModel_WhenEntityExists()
    {
        // Arrange
        var city = "Berlin";
        var entity = new LocationForecastEntity
        {
            Id = "berlin_20251104",
            City = city,
            DailyForecast = new[]
            {
                new DailyWeatherEntity { Id = "1", Date = new DateOnly(2025,11,4), TemperatureC = 10, Summary = "Cool" }
            }
        };

        _mockRepo.Setup(r => r.GetForecast(city)).ReturnsAsync(entity);

        // Act
        var result = await _service.GetForecast(city);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(city, result.City);
        Assert.Single(result.DailyForecast);
        _mockRepo.Verify(r => r.GetForecast(city), Times.Once);
    }

    [Fact]
    public async Task GetForecast_ReturnsNull_WhenEntityNotFound()
    {
        // Arrange
        var city = "Nowhere";
        _mockRepo.Setup(r => r.GetForecast(city)).ReturnsAsync((LocationForecastEntity?)null);

        // Act
        var result = await _service.GetForecast(city);

        // Assert
        Assert.Null(result);
        _mockRepo.Verify(r => r.GetForecast(city), Times.Once);
    }

    [Fact]
    public async Task CreateForecast_ReturnsRepositoryResult()
    {
        // Arrange
        var model = new LocationForecast { City = "Oslo" };
        var entity = model.ToEntity();
        var expectedId = "oslo_20251104";

        _mockRepo.Setup(r => r.CreateForecast(It.IsAny<LocationForecastEntity>()))
                 .ReturnsAsync(expectedId);

        // Act
        var result = await _service.CreateForecast(model);

        // Assert
        Assert.Equal(expectedId, result);
        _mockRepo.Verify(r => r.CreateForecast(It.IsAny<LocationForecastEntity>()), Times.Once);
    }

    [Fact]
    public async Task GetForecast_ThrowsException_WhenRepositoryFails()
    {
        // Arrange
        var city = "Berlin";
        _mockRepo.Setup(r => r.GetForecast(city))
                 .ThrowsAsync(new InvalidOperationException("Database failure"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetForecast(city));
        _mockRepo.Verify(r => r.GetForecast(city), Times.Once);
    }

    [Fact]
    public async Task CreateForecast_ThrowsException_WhenRepositoryFails()
    {
        // Arrange
        var model = new LocationForecast { City = "Oslo" };
        _mockRepo.Setup(r => r.CreateForecast(It.IsAny<LocationForecastEntity>()))
                 .ThrowsAsync(new Exception("Write failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.CreateForecast(model));
        _mockRepo.Verify(r => r.CreateForecast(It.IsAny<LocationForecastEntity>()), Times.Once);
    }

}
