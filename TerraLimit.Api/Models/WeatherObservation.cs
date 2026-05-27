using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TerraLimit.Api.Interfaces;

namespace TerraLimit.Api.Models;

public partial class WeatherObservation : IEntity<int>
{
    [Column("RecordId")]
    public int Id { get; set; }

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
