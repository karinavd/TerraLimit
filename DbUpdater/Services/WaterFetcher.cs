using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DbUpdater.Data;
using DbUpdater.Models;

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
                Delimiter = ";",
                HasHeaderRecord = false,
                MissingFieldFound = null, // Якщо колонки немає, просто ігноруємо
                HeaderValidated = null
            };

            using var reader = new StreamReader("./CSVData/aggregateddata_country.csv");
            using var csv = new CsvReader(reader, config);

            List<WaterRecord> records = [.. csv.GetRecords<WaterRecord>().Where(r => r.PhenomenonTimeReferenceYear == 2023)];

            _dbContext.WaterRecords.AddRange(records);
            await _dbContext.SaveChangesAsync();
        }
    }
}