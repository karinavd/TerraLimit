namespace TerraLimit.Model.Contracts
{
    public class AirQualityIndexDto
    {
        public int Id { get; set; }

        public decimal? Co { get; set; }

        public decimal? No2 { get; set; }

        public decimal? O3 { get; set; }

        public decimal? So2 { get; set; }

        public decimal? Pm25 { get; set; }

        public decimal? Pm10 { get; set; }

        public int? UsEpaIndex { get; set; }

        public int? GbDefraIndex { get; set; }
    }
}