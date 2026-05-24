using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace DbUpdater.Models
{
    public class WaterParameter
    {
        [Key]
        [Name("Parameter Code")]
        public string? Code { get; set; }

        [Name("Parameter Name")]
        public string? Name { get; set; }

        [Name("Parameter Description")]
        public string? Description { get; set; }

        [Ignore]
        public ICollection<WaterRecord> Measurements { get; set; } = new List<WaterRecord>();
    }
}