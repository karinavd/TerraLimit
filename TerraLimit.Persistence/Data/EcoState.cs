using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TerraLimit.Model.Entities;

namespace TerraLimit.Persistence.Data;

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

    public virtual DbSet<WaterParameter> WaterParameters { get; set; }

    public virtual DbSet<WaterRecord> WaterRecords { get; set; }

    public virtual DbSet<WaterStation> WaterStations { get; set; }

    public virtual DbSet<WeatherLocation> WeatherLocations { get; set; }

    public virtual DbSet<WeatherObservation> WeatherObservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AirQualityIndex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AirQuality_Indexes_pkey");

            entity.ToTable("AirQuality_Indexes");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Co).HasColumnName("CO");
            entity.Property(e => e.GbDefraIndex).HasColumnName("GB_DEFRA_Index");
            entity.Property(e => e.No2).HasColumnName("NO2");
            entity.Property(e => e.Pm10).HasColumnName("PM10");
            entity.Property(e => e.Pm25).HasColumnName("PM25");
            entity.Property(e => e.So2).HasColumnName("SO2");
            entity.Property(e => e.UsEpaIndex).HasColumnName("US_EPA_Index");

            entity.HasOne(d => d.Record).WithOne(p => p.AirQualityIndex)
                .HasForeignKey<AirQualityIndex>(d => d.Id)
                .HasConstraintName("fk_aq_obs");
        });

        modelBuilder.Entity<AtmosphereMetric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Atmosphere_Metrics_pkey");

            entity.ToTable("Atmosphere_Metrics");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CloudCoverPct).HasColumnName("Cloud_Cover_pct");
            entity.Property(e => e.FeelsLikeC).HasColumnName("FeelsLike_C");
            entity.Property(e => e.HumidityPct).HasColumnName("Humidity_pct");
            entity.Property(e => e.PrecipitationMm).HasColumnName("Precipitation_mm");
            entity.Property(e => e.PressureMb).HasColumnName("Pressure_mb");
            entity.Property(e => e.TemperatureC).HasColumnName("Temperature_C");
            entity.Property(e => e.UvIndex).HasColumnName("Uv_Index");
            entity.Property(e => e.WindDegree).HasColumnName("Wind_Degree");
            entity.Property(e => e.WindDirection).HasColumnName("Wind_Direction");
            entity.Property(e => e.WindSpeedKph).HasColumnName("Wind_Speed_kph");

            entity.HasOne(d => d.Record).WithOne(p => p.AtmosphereMetric)
                .HasForeignKey<AtmosphereMetric>(d => d.Id)
                .HasConstraintName("fk_metrics_obs");
        });

        modelBuilder.Entity<WaterParameter>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<WaterRecord>(entity =>
        {
            entity.HasIndex(e => e.ParameterCode, "IX_WaterRecords_ParameterCode");

            entity.HasIndex(e => e.StationId, "IX_WaterRecords_StationId");

            entity.HasOne(d => d.ParameterCodeNavigation).WithMany(p => p.WaterRecords).HasForeignKey(d => d.ParameterCode);

            entity.HasOne(d => d.Station).WithMany(p => p.WaterRecords).HasForeignKey(d => d.StationId);
        });

        modelBuilder.Entity<WeatherLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Weather_Locations_pkey");

            entity.ToTable("Weather_Locations");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<WeatherObservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Weather_Observations_pkey");

            entity.ToTable("Weather_Observations");

            entity.HasIndex(e => e.LocationId, "IX_Weather_Observations_LocationId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ConditionCode).HasColumnName("Condition_Code");
            entity.Property(e => e.ConditionText).HasColumnName("Condition_Text");
            entity.Property(e => e.CurrentConditionIcon).HasColumnName("Current_Condition_Icon");
            entity.Property(e => e.LastUpdated).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Localtime).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Location).WithMany(p => p.WeatherObservations)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_obs_location");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
