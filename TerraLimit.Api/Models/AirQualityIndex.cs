using System;
using System.Collections.Generic;

namespace TerraLimit.Api.Models;

public partial class AirQualityIndex
{
    public int RecordId { get; set; }

    public decimal? Co { get; set; }

    public decimal? No2 { get; set; }

    public decimal? O3 { get; set; }

    public decimal? So2 { get; set; }

    public decimal? Pm25 { get; set; }

    public decimal? Pm10 { get; set; }

    public int? UsEpaIndex { get; set; }

    public int? GbDefraIndex { get; set; }

    public virtual WeatherObservation Record { get; set; } = null!;
}
