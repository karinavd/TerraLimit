using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace DbUpdater.Models
{
    public class WaterStation
    {
        [Key]
        [Name("GEMS Station Number")]
        public string? Id { get; set; }

        [Name("Country Name")]
        public string? CountryName { get; set; }

        [Name("Latitude")]
        public double Latitude { get; set; }

        [Name("Longitude")]
        public double Longitude { get; set; }

        [Name("Water Type")]
        public string? WaterType { get; set; }

        [Name("Station Identifier")]
        public string? StationIdentifier { get; set; }

        [Ignore]
        public ICollection<WaterRecord> Measurements { get; set; } = new List<WaterRecord>();
    }
}