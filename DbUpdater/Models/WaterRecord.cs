using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CsvHelper.Configuration.Attributes;

namespace DbUpdater.Models
{
    public class WaterRecord
    {
        private DateTime _sampleDate;

        [Key]
        [Ignore]
        public int Id { get; set; }

        [Name("GEMS Station Number")]
        public string? StationId { get; set; }

        [Name("Sample Date")]
        public DateTime SampleDate
        {
            get => _sampleDate;
            set => _sampleDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        [Name("Depth")]
        public double Depth { get; set; }

        [Name("Parameter Code")]
        public string? ParameterCode { get; set; }

        [Name("Value")]
        public double? Value { get; set; }

        [Name("Unit")]
        public string? Unit { get; set; }

        [Ignore]
        [ForeignKey("StationId")]
        public WaterStation? Station { get; set; }
    }
}