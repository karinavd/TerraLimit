using DbUpdater.Data;
using DbUpdater.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EcoDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<Fetcher>();
builder.Services.AddScoped<WaterFetcher>();

builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var fetcher = scope.ServiceProvider.GetRequiredService<Fetcher>();
    var waterFetcher = scope.ServiceProvider.GetRequiredService<WaterFetcher>();
    await waterFetcher.GetWaterDataAsync();
    await fetcher.UpdateWeatherDataAsync();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/", () => "Fetch is succesfully done.");

app.Run();
