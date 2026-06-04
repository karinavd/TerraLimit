namespace TerraLimit.Model.Contracts
{
    public class WeatherObservationDto
    {
        public int Id { get; set; }

        public int? LocationId { get; set; }

        public DateTime? Localtime { get; set; }
        public string? ConditionText { get; set; }

        public string? CurrentConditionIcon { get; set; }
    }
}