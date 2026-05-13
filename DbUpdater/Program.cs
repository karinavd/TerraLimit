using DbUpdater.Data;
using DbUpdater.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<EcoDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<Fetcher>();
builder.Services.AddHttpClient();

var app = builder.Build();

var cityNames = await File.ReadAllLinesAsync("./worldcities.csv");
var validCitiesName = cityNames
    .Select(x => x.Trim().Replace("’", "'").Replace("‘", "'"))
    .Where(x => !string.IsNullOrWhiteSpace(x))
    .ToArray();

await Parallel.ForEachAsync(validCitiesName, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (city, token) =>
{
    try
    {
        using var scope = app.Services.CreateScope();

        var fetcher = scope.ServiceProvider.GetRequiredService<Fetcher>();

        await fetcher.UpdateDataAsync(city);
    }
    catch (Exception ex)
    {
        System.Console.WriteLine(ex.Message);
    }
});

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.MapGet("/", () => "Fetch is succesfully done.");

app.Run();
