using BoringTemplate.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<LocationForecastEntity> WeatherForecasts { get; set; }
    public DbSet<DailyWeatherEntity> DailyForecasts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LocationForecastEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.City);
            entity.Property(e => e.City)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.HasMany(e => e.DailyForecast)
                  .WithOne()
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DailyWeatherEntity>(entity =>
        {
            entity.Property(e => e.Date)
                  .IsRequired()
                  .HasConversion(
                      d => d.ToDateTime(TimeOnly.MinValue),
                      d => DateOnly.FromDateTime(d));

            entity.Property(e => e.TemperatureC).IsRequired();
            entity.Property(e => e.Summary).HasMaxLength(100);
        });
    }
}
