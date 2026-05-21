using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherRecords");
        }
    }
}
