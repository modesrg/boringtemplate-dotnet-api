using BoringTemplate.API.DAL.Repositories;
using BoringTemplate.API.DAL.Repositories.Interface;
using BoringTemplate.API.Demo;
using BoringTemplate.API.Services;
using BoringTemplate.API.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

//Add Services
builder.Services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var sqliteCSB = new SqliteConnectionStringBuilder("Data Source=local.db");
sqliteCSB.DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sqliteCSB.DataSource);
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(sqliteCSB.ToString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed the Demo DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureDeletedAsync();
    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedIfEmptyAsync(db);
}

app.Run();
