namespace TerraLimit.Model.Contracts
{
    public class WaterRecordDto
    {
        public int Id { get; set; }

        public string? StationId { get; set; }

        public DateOnly SampleDate { get; set; }

        public double Depth { get; set; }

        public string? ParameterCode { get; set; }
        public string? ParameterName { get; set; }
        public string? ParameterDescription { get; set; }

        public decimal? Value { get; set; }

        public string? Unit { get; set; }
    }
}