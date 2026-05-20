using System;
using System.Collections.Generic;

namespace TerraLimit.Api.Models;

public partial class WeatherLocation
{
    public int LocationId { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? Country { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? Timezone { get; set; }

    public virtual ICollection<WeatherObservation> WeatherObservations { get; set; } = new List<WeatherObservation>();
}
