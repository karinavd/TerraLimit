using System.Globalization;
using System.IO.Compression;
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
        private readonly HttpClient _httpClient;

        public WaterFetcher(EcoDb dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
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
            var tempFileName = Path.GetTempFileName();

            try
            {
                using (var netStream = await _httpClient.GetStreamAsync("https://zenodo.org/records/18459694/files/GFQA_v3.zip?download=1"))
                using (var fs = new FileStream(tempFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await netStream.CopyToAsync(fs);
                }

                using var archive = ZipFile.OpenRead(tempFileName);
                var stationsFile = archive.Entries.FirstOrDefault(e => e.Name.Equals("GEMStat_station_metadata.csv", StringComparison.OrdinalIgnoreCase));

                using var stationsFileStream = stationsFile.Open();

                using var stationReader = new StreamReader(stationsFileStream);
                using var stationCsv = new CsvReader(stationReader, config);

                var waterStations = stationCsv.GetRecords<WaterStation>().DistinctBy(s => s.Id).ToList();
                var existingWStations = new HashSet<string>(await _dbContext.WaterStations.Select(s => s.Id).ToListAsync());

                foreach (var station in waterStations)
                {
                    if (!existingWStations.Contains(station.Id))
                    {
                        _dbContext.WaterStations.Add(station);
                        existingWStations.Add(station.Id);
                    }
                }

                await _dbContext.SaveChangesAsync();

                // water measurements data
                string[] fileNames = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "CSVData", "waterdataFileNames.txt"));

                foreach (var fName in fileNames)
                {
                    var waterQualityFile = archive.Entries.FirstOrDefault(e => e.Name.Equals(fName, StringComparison.OrdinalIgnoreCase));
                    if (waterQualityFile == null) continue;

                    using var waterQualityFileStream = waterQualityFile.Open();

                    using var waterReader = new StreamReader(waterQualityFileStream);
                    using var waterCsv = new CsvReader(waterReader, config);

                    var latestWaterRecords = new Dictionary<string, WaterRecord>();

                    foreach (var rec in waterCsv.GetRecords<WaterRecord>())
                    {
                        latestWaterRecords[rec.StationId] = rec;
                    }

                    if (latestWaterRecords.Count == 0) continue;

                    var currParamCodes = latestWaterRecords.Values.Select(r => r.ParameterCode)
                                                                  .Distinct()
                                                                  .ToArray();
                    var existingWRecordsList = await _dbContext.WaterRecords.Where(r => currParamCodes.Contains(r.ParameterCode))
                                                                        .Select(r => r.StationId)
                                                                        .ToListAsync();
                    var existingWRecords = new HashSet<string>(existingWRecordsList);

                    foreach (var record in latestWaterRecords.Values)
                    {
                        if (!existingWRecords.Contains(record.StationId))
                        {
                            _dbContext.WaterRecords.Add(record);
                            existingWRecords.Add(record.StationId);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                    _dbContext.ChangeTracker.Clear();
                }
            }
            finally
            {
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
            }
        }
    }
}