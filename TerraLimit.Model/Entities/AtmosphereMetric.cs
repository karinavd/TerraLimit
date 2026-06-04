using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TerraLimit.Model.Interfaces;

namespace TerraLimit.Model.Entities;

public partial class AtmosphereMetric : IEntity<int>
{
    [Column("RecordId")]
    public int Id { get; set; }

    public decimal? TemperatureC { get; set; }

    public decimal? FeelsLikeC { get; set; }

    public int? HumidityPct { get; set; }

    public decimal? WindSpeedKph { get; set; }

    public int? WindDegree { get; set; }

    public string? WindDirection { get; set; }

    public decimal? PressureMb { get; set; }

    public decimal? PrecipitationMm { get; set; }

    public int? CloudCoverPct { get; set; }

    public decimal? UvIndex { get; set; }

    public virtual WeatherObservation Record { get; set; } = null!;
}
