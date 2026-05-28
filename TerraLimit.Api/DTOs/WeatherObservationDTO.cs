using TerraLimit.Api.Models;

namespace TerraLimit.Api.DTOs;

public class WeatherObservationDTO
{
    public int Id { get; set; }

    public int? LocationId { get; set; }

    public DateTime? Localtime { get; set; }
    public string? ConditionText { get; set; }

    public string? CurrentConditionIcon { get; set; }
}