using System;
using System.Collections.Generic;

namespace DbUpdater.Models;

public class WeatherObservation
{
    public int RecordId { get; set; }

    public int? LocationId { get; set; }

    public DateTime? Localtime { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string? ConditionText { get; set; }

    public int? ConditionCode { get; set; }

    public string? CurrentConditionIcon { get; set; }

    public virtual AirQualityIndex? AirQualityIndex { get; set; }

    public virtual AtmosphereMetric? AtmosphereMetric { get; set; }

    public virtual WeatherLocation? Location { get; set; }
}
