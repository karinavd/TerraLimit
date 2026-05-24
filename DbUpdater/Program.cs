using System.Reflection;
using DbUpdater.Data;
using DbUpdater.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    // .AddUserSecrets(Assembly.GetExecutingAssembly())
    .Build();

var builder = new ServiceCollection();

builder.AddSingleton<IConfiguration>(configuration);

builder.AddDbContext<EcoDb>(options =>
    options.UseNpgsql(configuration.GetConnectionString("Default")));

builder.AddScoped<Fetcher>();
builder.AddScoped<WaterFetcher>();
builder.AddHttpClient();

var app = builder.BuildServiceProvider();

using (var migrationScope = app.CreateScope())
{
    var dbContext = migrationScope.ServiceProvider.GetRequiredService<EcoDb>();
    await dbContext.Database.MigrateAsync();
}

using var scope = app.CreateScope();
var fetcher = scope.ServiceProvider.GetRequiredService<Fetcher>();
var waterFetcher = scope.ServiceProvider.GetRequiredService<WaterFetcher>();

System.Console.WriteLine("Db updater started");

System.Console.WriteLine("Water data fetching started...");
await waterFetcher.GetWaterDataAsync();
System.Console.WriteLine("Water data fetching ended.");

System.Console.WriteLine("Weather data fetching started...");
await fetcher.UpdateWeatherDataAsync();
System.Console.WriteLine("Weather data fetching ended.");

System.Console.WriteLine("Db updated");