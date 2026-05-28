namespace TerraLimit.Api.DTOs
{
    class WaterStationDTO
    {
        public string Id { get; set; } = null!;

        public string? CountryName { get; set; }

        public string? WaterType { get; set; }

        public string? StationIdentifier { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}