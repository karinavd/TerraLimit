using DbUpdater.Data;
using DbUpdater.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace DbUpdater.Services
{
    public class Fetcher
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _apiKey;

        public Fetcher(HttpClient httpClient, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _serviceProvider = serviceProvider;
            _apiKey = configuration["WeatherApi:ApiKey"];
        }

        public async Task UpdateWeatherDataAsync()
        {
            var cityNames = await File.ReadAllLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "CSVData", "worldcities.txt"));
            var validCitiesName = cityNames
                .Select(x => x.Trim().Replace("’", "'").Replace("‘", "'"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            Dictionary<string, string> requiredWeatherData;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EcoDb>();

                var rawDbData = await dbContext.WeatherRecords
                    .Select(r => new { r.Location.Name, r.Current.LastUpdated })
                    .ToListAsync();

                requiredWeatherData = rawDbData
                    .DistinctBy(x => x.Name)
                    .ToDictionary(x => x.Name, x => x.LastUpdated);
            }

            await Parallel.ForEachAsync(validCitiesName, new ParallelOptions { MaxDegreeOfParallelism = 50 }, async (city, token) =>
            {
                try
                {
                    var fetchedRecord = await _httpClient.GetFromJsonAsync<WeatherRecord>
                        ($"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={city}&aqi=yes", token);

                    if (fetchedRecord == null || fetchedRecord.Location == null) return;

                    if (requiredWeatherData.TryGetValue(fetchedRecord.Location.Name, out var savedLastUpdated))
                    {
                        if (savedLastUpdated == fetchedRecord.Current.LastUpdated) return;
                    }

                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<EcoDb>();

                    await UpdateConcreteWeatherAsync(fetchedRecord, dbContext);
                }
                catch { }
            });
        }

        private static async Task UpdateConcreteWeatherAsync(WeatherRecord fetchedRecord, EcoDb _dbContext)
        {
            var existingRecord = await _dbContext.WeatherRecords.FirstOrDefaultAsync(record => record.Location.Name == fetchedRecord.Location.Name);

            if (existingRecord != null)
            {
                if (existingRecord.Current.LastUpdated != fetchedRecord.Current.LastUpdated)
                {
                    _dbContext.Entry(existingRecord.Location).CurrentValues.SetValues(fetchedRecord.Location);
                    _dbContext.Entry(existingRecord.Current).CurrentValues.SetValues(fetchedRecord.Current);
                    _dbContext.Entry(existingRecord.Current.AirQuality).CurrentValues.SetValues(fetchedRecord.Current.AirQuality);
                    _dbContext.Entry(existingRecord.Current.Condition).CurrentValues.SetValues(fetchedRecord.Current.Condition);
                }
            }
            else
            {
                _dbContext.WeatherRecords.Add(fetchedRecord);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}