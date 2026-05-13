using DbUpdater.Data;
using DbUpdater.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUpdater.Services
{
    public class Fetcher
    {
        private readonly HttpClient _httpClient;
        private readonly EcoDb _dbContext;
        private readonly string _apiKey = "43a9c96070de402787d60222261205";

        public Fetcher(HttpClient httpClient, EcoDb ecoDb)
        {
            _httpClient = httpClient;
            _dbContext = ecoDb;
        }

        public async Task UpdateDataAsync(string cityName)
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