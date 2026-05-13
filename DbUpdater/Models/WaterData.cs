using CsvHelper.Configuration.Attributes;

namespace DbUpdater.Models
{
    public class WaterRecord
    {
        [Ignore]
        public int Id { get; set; }

        [Name("countryGroup")]
        public string CountryGroup { get; set; }

        [Name("countryCode")]
        public string CountryCode { get; set; }

        [Name("countryName")]
        public string CountryName { get; set; }

        [Name("waterBodyCategory")]
        public string WaterBodyCategory { get; set; }

        [Name("eeaIndicator")]
        public string EeaIndicator { get; set; }

        [Name("phenomenonTimeReferenceYear")]
        public int? PhenomenonTimeReferenceYear { get; set; }

        [Name("resultUom")]
        public string ResultUom { get; set; }

        [Name("meanValue")]
        public double? MeanValue { get; set; }

        [Name("stdevValue")]
        public double? StdevValue { get; set; }

        [Name("minValue")]
        public double? MinValue { get; set; }

        [Name("maxValue")]
        public double? MaxValue { get; set; }

        [Name("class1")]
        public string Class1 { get; set; }

        [Name("class2")]
        public string Class2 { get; set; }

        [Name("class3")]
        public string Class3 { get; set; }

        [Name("class4")]
        public string Class4 { get; set; }

        [Name("class5")]
        public string Class5 { get; set; }

        [Name("numberOfSites")]
        public int? NumberOfSites { get; set; }

        [Name("numberOfReportedSites")]
        public int? NumberOfReportedSites { get; set; }

        [Name("lat")]
        public double? Lat { get; set; }

        [Name("lon")]
        public double? Lon { get; set; }
    }
}