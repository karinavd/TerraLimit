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
            var cityNames = await File.ReadAllLinesAsync("./CSVData/worldcities.csv");
            var validCitiesName = cityNames
                .Select(x => x.Trim().Replace("’", "'").Replace("‘", "'"))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            await Parallel.ForEachAsync(validCitiesName, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (city, token) =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<EcoDb>();

                    await UpdateConcreteWeatherAsync(city, dbContext);
                }
                catch { }
            });
        }

        public async Task UpdateConcreteWeatherAsync(string cityName, EcoDb _dbContext)
        {
            var fetchedRecord = await _httpClient.GetFromJsonAsync<WeatherRecord>
                ($"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={cityName}&aqi=yes");

            if (fetchedRecord == null) return;

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