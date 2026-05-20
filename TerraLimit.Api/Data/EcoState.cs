using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerraLimit.Api.Models;

namespace TerraLimit.Api.Data;

public partial class EcoState : DbContext
{
    public EcoState()
    {
    }

    public EcoState(DbContextOptions<EcoState> options)
        : base(options)
    {
    }

    public virtual DbSet<AirQualityIndex> AirQualityIndexes { get; set; }

    public virtual DbSet<AtmosphereMetric> AtmosphereMetrics { get; set; }

    public virtual DbSet<WeatherLocation> WeatherLocations { get; set; }

    public virtual DbSet<WeatherObservation> WeatherObservations { get; set; }
}
