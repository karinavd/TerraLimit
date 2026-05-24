using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterParameters",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterParameters", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "WaterStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    WaterType = table.Column<string>(type: "text", nullable: false),
                    StationIdentifier = table.Column<string>(type: "text", nullable: false),
                    WaterBodyName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weather_Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    City = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Timezone = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Weather_Locations_pkey", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "WeatherRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Location_Name = table.Column<string>(type: "text", nullable: false),
                    Location_Region = table.Column<string>(type: "text", nullable: false),
                    Location_Country = table.Column<string>(type: "text", nullable: false),
                    Location_Lat = table.Column<double>(type: "double precision", nullable: false),
                    Location_Lon = table.Column<double>(type: "double precision", nullable: false),
                    Location_TzId = table.Column<string>(type: "text", nullable: false),
                    Location_LocaltimeEpoch = table.Column<long>(type: "bigint", nullable: false),
                    Location_Localtime = table.Column<string>(type: "text", nullable: false),
                    Current_LastUpdatedEpoch = table.Column<long>(type: "bigint", nullable: false),
                    Current_LastUpdated = table.Column<string>(type: "text", nullable: false),
                    Current_TempC = table.Column<double>(type: "double precision", nullable: false),
                    Current_TempF = table.Column<double>(type: "double precision", nullable: false),
                    Current_IsDay = table.Column<int>(type: "integer", nullable: false),
                    Current_Condition_Text = table.Column<string>(type: "text", nullable: false),
                    Current_Condition_Icon = table.Column<string>(type: "text", nullable: false),
                    Current_Condition_Code = table.Column<int>(type: "integer", nullable: false),
                    Current_WindMph = table.Column<double>(type: "double precision", nullable: false),
                    Current_WindKph = table.Column<double>(type: "double precision", nullable: false),
                    Current_WindDegree = table.Column<int>(type: "integer", nullable: false),
                    Current_WindDir = table.Column<string>(type: "text", nullable: false),
                    Current_PressureMb = table.Column<double>(type: "double precision", nullable: false),
                    Current_PressureIn = table.Column<double>(type: "double precision", nullable: false),
                    Current_PrecipMm = table.Column<double>(type: "double precision", nullable: false),
                    Current_PrecipIn = table.Column<double>(type: "double precision", nullable: false),
                    Current_Humidity = table.Column<int>(type: "integer", nullable: false),
                    Current_Cloud = table.Column<int>(type: "integer", nullable: false),
                    Current_FeelslikeC = table.Column<double>(type: "double precision", nullable: false),
                    Current_FeelslikeF = table.Column<double>(type: "double precision", nullable: false),
                    Current_Uv = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_Co = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_No2 = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_O3 = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_So2 = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_Pm25 = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_Pm10 = table.Column<double>(type: "double precision", nullable: false),
                    Current_AirQuality_UsEpaIndex = table.Column<int>(type: "integer", nullable: false),
                    Current_AirQuality_GbDefraIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaterRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StationId = table.Column<string>(type: "text", nullable: false),
                    SampleDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Depth = table.Column<double>(type: "double precision", nullable: false),
                    ParameterCode = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    WaterParameterCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterRecords_WaterParameters_WaterParameterCode",
                        column: x => x.WaterParameterCode,
                        principalTable: "WaterParameters",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_WaterRecords_WaterStations_StationId",
                        column: x => x.StationId,
                        principalTable: "WaterStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weather_Observations",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<int>(type: "integer", nullable: true),
                    Localtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Condition_Text = table.Column<string>(type: "text", nullable: true),
                    Condition_Code = table.Column<int>(type: "integer", nullable: true),
                    Current_Condition_Icon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Weather_Observations_pkey", x => x.RecordId);
                    table.ForeignKey(
                        name: "fk_obs_location",
                        column: x => x.LocationId,
                        principalTable: "Weather_Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirQuality_Indexes",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "integer", nullable: false),
                    CO = table.Column<decimal>(type: "numeric", nullable: true),
                    NO2 = table.Column<decimal>(type: "numeric", nullable: true),
                    O3 = table.Column<decimal>(type: "numeric", nullable: true),
                    SO2 = table.Column<decimal>(type: "numeric", nullable: true),
                    PM25 = table.Column<decimal>(type: "numeric", nullable: true),
                    PM10 = table.Column<decimal>(type: "numeric", nullable: true),
                    US_EPA_Index = table.Column<int>(type: "integer", nullable: true),
                    GB_DEFRA_Index = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AirQuality_Indexes_pkey", x => x.RecordId);
                    table.ForeignKey(
                        name: "fk_aq_obs",
                        column: x => x.RecordId,
                        principalTable: "Weather_Observations",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Atmosphere_Metrics",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "integer", nullable: false),
                    Temperature_C = table.Column<decimal>(type: "numeric", nullable: true),
                    FeelsLike_C = table.Column<decimal>(type: "numeric", nullable: true),
                    Humidity_pct = table.Column<int>(type: "integer", nullable: true),
                    Wind_Speed_kph = table.Column<decimal>(type: "numeric", nullable: true),
                    Wind_Degree = table.Column<int>(type: "integer", nullable: true),
                    Wind_Direction = table.Column<string>(type: "text", nullable: true),
                    Pressure_mb = table.Column<decimal>(type: "numeric", nullable: true),
                    Precipitation_mm = table.Column<decimal>(type: "numeric", nullable: true),
                    Cloud_Cover_pct = table.Column<int>(type: "integer", nullable: true),
                    Uv_Index = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Atmosphere_Metrics_pkey", x => x.RecordId);
                    table.ForeignKey(
                        name: "fk_metrics_obs",
                        column: x => x.RecordId,
                        principalTable: "Weather_Observations",
                        principalColumn: "RecordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterRecords_StationId",
                table: "WaterRecords",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterRecords_WaterParameterCode",
                table: "WaterRecords",
                column: "WaterParameterCode");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_Observations_LocationId",
                table: "Weather_Observations",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQuality_Indexes");

            migrationBuilder.DropTable(
                name: "Atmosphere_Metrics");

            migrationBuilder.DropTable(
                name: "WaterRecords");

            migrationBuilder.DropTable(
                name: "WeatherRecords");

            migrationBuilder.DropTable(
                name: "Weather_Observations");

            migrationBuilder.DropTable(
                name: "WaterParameters");

            migrationBuilder.DropTable(
                name: "WaterStations");

            migrationBuilder.DropTable(
                name: "Weather_Locations");
        }
    }
}
