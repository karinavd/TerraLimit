using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DbUpdater.Data;
using DbUpdater.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUpdater.Services
{
    public class WaterFetcher
    {
        private readonly EcoDb _dbContext;

        public WaterFetcher(EcoDb dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task GetWaterDataAsync()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            // water stations
            using (var stationReader = new StreamReader("./CSVData/GEMStat_station_metadata.csv"))
            using (var stationCsv = new CsvReader(stationReader, config))
            {
                var waterStations = stationCsv.GetRecords<WaterStation>().ToList();

                foreach (var station in waterStations)
                {
                    if (!_dbContext.WaterStations.Any(s => s.Id == station.Id))
                    {
                        _dbContext.WaterStations.Add(station);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }

            // water measurements data
            var pathToWaterData = Path.Combine(Directory.GetCurrentDirectory(), "CSVData", "WaterMineralsData");
            string[] fileInfos = Directory.GetFiles(pathToWaterData);

            foreach (var filePath in fileInfos)
            {
                var fileName = filePath.Split("\\").Last();

                using var waterReader = new StreamReader($"./CSVData/WaterMineralsData/{fileName}");
                using var waterCsv = new CsvReader(waterReader, config);

                var allWaterRecords = waterCsv.GetRecords<WaterRecord>();
                var waterRecords = allWaterRecords.AsEnumerable().Reverse().DistinctBy(x => x.StationId).Reverse().ToList();

                foreach (var record in waterRecords)
                {
                    var existingRecord = await _dbContext.WaterRecords.FirstOrDefaultAsync(r => r.ParameterCode == record.ParameterCode
                                                                                             && r.StationId == record.StationId);
                    if (existingRecord == null)
                    {
                        _dbContext.WaterRecords.Add(record);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}