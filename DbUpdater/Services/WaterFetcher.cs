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
        private readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
            HasHeaderRecord = true,
            MissingFieldFound = null,
            HeaderValidated = null
        };

        public WaterFetcher(EcoDb dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        public async Task GetWaterDataAsync()
        {
            var tempFileName = Path.GetTempFileName();

            try
            {
                using (var netStream = await _httpClient.GetStreamAsync("https://zenodo.org/records/18459694/files/GFQA_v3.zip?download=1"))
                using (var fs = new FileStream(tempFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await netStream.CopyToAsync(fs);
                }

                using var archive = ZipFile.OpenRead(tempFileName);

                await ProcessWaterParametersAsync(archive);
                await ProcessWaterStationsAsync(archive);
                await ProcessWaterRecordsAsync(archive);
            }
            finally
            {
                if (File.Exists(tempFileName))
                {
                    File.Delete(tempFileName);
                }
            }
        }

        private async Task ProcessWaterParametersAsync(ZipArchive archive)
        {
            var paramFile = archive.Entries.FirstOrDefault(e => e.Name.Equals("GEMStat_parameter_metadata.csv", StringComparison.OrdinalIgnoreCase));
            if (paramFile == null) return;

            using var stream = paramFile.Open();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, _csvConfig);

            var parameters = csv.GetRecords<WaterParameter>().DistinctBy(p => p.Code).ToList();
            var existingParams = new HashSet<string?>(await _dbContext.WaterParameters.Select(p => p.Code).ToListAsync());

            foreach (var param in parameters)
            {
                if (param.Code != null && !existingParams.Contains(param.Code))
                {
                    _dbContext.WaterParameters.Add(param);
                    existingParams.Add(param.Code);
                }
            }

            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();
        }

        private async Task ProcessWaterStationsAsync(ZipArchive archive)
        {
            var stationsFile = archive.Entries.FirstOrDefault(e => e.Name.Equals("GEMStat_station_metadata.csv", StringComparison.OrdinalIgnoreCase));
            if (stationsFile == null) return;

            using var stream = stationsFile.Open();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, _csvConfig);

            var waterStations = csv.GetRecords<WaterStation>().DistinctBy(s => s.Id).ToList();
            var existingWStations = new HashSet<string?>(await _dbContext.WaterStations.Select(s => s.Id).ToListAsync());

            foreach (var station in waterStations)
            {
                if (station.Id != null && !existingWStations.Contains(station.Id))
                {
                    _dbContext.WaterStations.Add(station);
                    existingWStations.Add(station.Id);
                }
            }

            await _dbContext.SaveChangesAsync();
            _dbContext.ChangeTracker.Clear();
        }

        private async Task ProcessWaterRecordsAsync(ZipArchive archive)
        {
            string[] fileNames = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "CSVData", "waterdataFileNames.txt"));

            foreach (var fName in fileNames)
            {
                var waterQualityFile = archive.Entries.FirstOrDefault(e => e.Name.Equals(fName, StringComparison.OrdinalIgnoreCase));
                if (waterQualityFile == null) continue;

                using var stream = waterQualityFile.Open();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, _csvConfig);

                var latestWaterRecords = new Dictionary<string, WaterRecord>();

                foreach (var rec in csv.GetRecords<WaterRecord>())
                {
                    if (rec.StationId != null)
                    {
                        latestWaterRecords[rec.StationId] = rec;
                    }
                }

                if (latestWaterRecords.Count == 0) continue;

                var currParamCodes = latestWaterRecords.Values.Select(r => r.ParameterCode).Distinct().ToArray();

                var existingWRecordsList = await _dbContext.WaterRecords
                    .Where(r => currParamCodes.Contains(r.ParameterCode))
                    .Select(r => r.StationId)
                    .ToListAsync();

                var existingWRecords = new HashSet<string?>(existingWRecordsList);

                foreach (var record in latestWaterRecords.Values)
                {
                    if (record.StationId != null && !existingWRecords.Contains(record.StationId))
                    {
                        _dbContext.WaterRecords.Add(record);
                        existingWRecords.Add(record.StationId);
                    }
                }

                await _dbContext.SaveChangesAsync();

                _dbContext.ChangeTracker.Clear();
            }
        }
    }
}