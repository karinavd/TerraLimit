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
            var cityCoordinates = await File.ReadAllLinesAsync(Path.Combine(Directory.GetCurrentDirectory(), "CSVData", "worldcities.txt"));

            Dictionary<string, string> requiredWeatherData;
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<EcoDb>();

                var rawDbData = await dbContext.WeatherRecords
                    .Select(r => new { r.Location!.Name, r.Current!.LastUpdated })
                    .ToListAsync();

                requiredWeatherData = rawDbData
                    .DistinctBy(x => x.Name)
                    .ToDictionary(x => x.Name!, x => x.LastUpdated)!;
            }

            await Parallel.ForEachAsync(cityCoordinates, new ParallelOptions { MaxDegreeOfParallelism = 20 }, async (cityCoordinates, token) =>
            {
                try
                {
                    var response = await _httpClient
                                         .GetAsync($"https://api.weatherapi.com/v1/current.json?key={_apiKey}&q={cityCoordinates}&aqi=yes", token);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Skipped: {cityCoordinates} (Code: {response.StatusCode})");
                        return;
                    }

                    var fetchedRecord = await response.Content.ReadFromJsonAsync<WeatherRecord>(token);

                    if (fetchedRecord == null || fetchedRecord.Location == null) return;

                    if (requiredWeatherData.TryGetValue(fetchedRecord.Location.Name!, out var savedLastUpdated))
                    {
                        if (savedLastUpdated == fetchedRecord.Current!.LastUpdated) return;
                    }

                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<EcoDb>();

                    await UpdateConcreteWeatherAsync(fetchedRecord, dbContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error for {cityCoordinates}: {ex.Message}");
                }
            });
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<EcoDb>();

                dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));

                var weatherLocations = @"
        INSERT INTO ""Weather_Locations"" (
            ""LocationId"", ""City"", ""Region"", ""Country"", ""Latitude"", ""Longitude"", ""Timezone""
        )
        SELECT DISTINCT ON (""Location_Name"", ""Location_Region"", ""Location_Country"") 
            ""Id"" AS ""LocationId"",
            ""Location_Name"" AS ""City"",
            ""Location_Region"" AS ""Region"",
            ""Location_Country"" AS ""Country"",
            ""Location_Lat"" AS ""Latitude"",
            ""Location_Lon"" AS ""Longitude"",
            ""Location_TzId"" AS ""Timezone""
        FROM ""WeatherRecords""
        ORDER BY ""Location_Name"", ""Location_Region"", ""Location_Country"", ""Id""
        ON CONFLICT (""LocationId"") DO NOTHING;";

                await dbContext.Database.ExecuteSqlRawAsync(weatherLocations);

                var weatherObservations = @"
        INSERT INTO ""Weather_Observations"" (
            ""RecordId"", ""LocationId"", ""Localtime"", ""LastUpdated"", ""Condition_Text"", ""Condition_Code"", ""Current_Condition_Icon""
        )
        SELECT DISTINCT ON (wr.""Id"") 
            wr.""Id"" AS ""RecordId"",
            loc.""LocationId"" AS ""LocationId"",
            wr.""Location_Localtime""::timestamp without time zone AS ""Localtime"",
            wr.""Current_LastUpdated""::timestamp without time zone AS ""LastUpdated"",                
            wr.""Current_Condition_Text"" AS ""Condition_Text"",       
            wr.""Current_Condition_Code"" AS ""Condition_Code"",
            wr.""Current_Condition_Icon""
        FROM ""WeatherRecords"" wr
        JOIN ""Weather_Locations"" loc 
            ON wr.""Location_Name"" = loc.""City""
           AND (wr.""Location_Region"" = loc.""Region"" OR (wr.""Location_Region"" IS NULL AND loc.""Region"" IS NULL))
           AND wr.""Location_Country"" = loc.""Country""
        WHERE TRIM(wr.""Current_Condition_Text"") <> ''
        ORDER BY wr.""Id""
        ON CONFLICT (""RecordId"") DO NOTHING;";

                await dbContext.Database.ExecuteSqlRawAsync(weatherObservations);

                var atmosphereMetrics = @"
        INSERT INTO ""Atmosphere_Metrics"" (
            ""RecordId"", ""Temperature_C"", ""FeelsLike_C"", ""Humidity_pct"", ""Wind_Speed_kph"", ""Wind_Degree"", ""Wind_Direction"", ""Pressure_mb"", ""Precipitation_mm"", ""Cloud_Cover_pct"", ""Uv_Index""
        )
        SELECT 
            ""Id"" AS ""RecordId"",                               
            ""Current_TempC"" AS ""Temperature_C"",                  
            ""Current_FeelslikeC"" AS ""FeelsLike_C"",              
            ""Current_Humidity"" AS ""Humidity_pct"",                
            ""Current_WindKph"" AS ""Wind_Speed_kph"",                
            ""Current_WindDegree"" AS ""Wind_Degree"",              
            ""Current_WindDir"" AS ""Wind_Direction"",                
            ""Current_PressureMb"" AS ""Pressure_mb"",              
            ""Current_PrecipMm"" AS ""Precipitation_mm"",              
            ""Current_Cloud"" AS ""Cloud_Cover_pct"",                  
            ""Current_Uv"" AS ""Uv_Index""                      
        FROM ""WeatherRecords""
        WHERE ""Id"" IN (SELECT ""RecordId"" FROM ""Weather_Observations"")
        ON CONFLICT (""RecordId"") DO NOTHING;";

                await dbContext.Database.ExecuteSqlRawAsync(atmosphereMetrics);

                var airQualityIndexes = @"
        INSERT INTO ""AirQuality_Indexes"" (
            ""RecordId"", ""CO"", ""NO2"", ""O3"", ""SO2"", ""PM25"", ""PM10"", ""US_EPA_Index"", ""GB_DEFRA_Index""
        )
        SELECT 
            ""Id"" AS ""RecordId"",
            ""Current_AirQuality_Co"" AS ""CO"",
            ""Current_AirQuality_No2"" AS ""NO2"",
            ""Current_AirQuality_O3"" AS ""O3"",
            ""Current_AirQuality_So2"" AS ""SO2"",
            ""Current_AirQuality_Pm25"" AS ""PM25"",
            ""Current_AirQuality_Pm10"" AS ""PM10"",
            ""Current_AirQuality_UsEpaIndex"" AS ""US_EPA_Index"",
            ""Current_AirQuality_GbDefraIndex"" AS ""GB_DEFRA_Index""
        FROM ""WeatherRecords""
        WHERE ""Current_AirQuality_UsEpaIndex"" >= 0
          AND ""Id"" IN (SELECT ""RecordId"" FROM ""Weather_Observations"")
        ON CONFLICT (""RecordId"") DO NOTHING;";

                await dbContext.Database.ExecuteSqlRawAsync(airQualityIndexes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
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