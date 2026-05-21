using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class NewWaterStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class1",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Class2",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Class3",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Class4",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Class5",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "CountryGroup",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "EeaIndicator",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Lat",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Lon",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "MeanValue",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "NumberOfReportedSites",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "NumberOfSites",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "PhenomenonTimeReferenceYear",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "ResultUom",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "WaterBodyCategory",
                table: "WaterRecords");

            migrationBuilder.RenameColumn(
                name: "StdevValue",
                table: "WaterRecords",
                newName: "Value");

            migrationBuilder.AlterColumn<string>(
                name: "Location_TzId",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Location_Region",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Location_Name",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Location_Lon",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<long>(
                name: "Location_LocaltimeEpoch",
                table: "WeatherRecords",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Location_Localtime",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Location_Lat",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Location_Country",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<double>(
                name: "Current_WindMph",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_WindKph",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Current_WindDir",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Current_WindDegree",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Current_Uv",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_TempF",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_TempC",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_PressureMb",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_PressureIn",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_PrecipMm",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_PrecipIn",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<long>(
                name: "Current_LastUpdatedEpoch",
                table: "WeatherRecords",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Current_LastUpdated",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Current_IsDay",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Current_Humidity",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Current_FeelslikeF",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_FeelslikeC",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "Current_Condition_Text",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Current_Condition_Icon",
                table: "WeatherRecords",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Current_Condition_Code",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Current_Cloud",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Current_AirQuality_UsEpaIndex",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_So2",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_Pm25",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_Pm10",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_O3",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_No2",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "Current_AirQuality_GbDefraIndex",
                table: "WeatherRecords",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_Co",
                table: "WeatherRecords",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<double>(
                name: "Depth",
                table: "WaterRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ParameterCode",
                table: "WaterRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SampleDate",
                table: "WaterRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StationId",
                table: "WaterRecords",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "WaterRecords",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WaterStations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: true),
                    WaterType = table.Column<string>(type: "text", nullable: true),
                    StationIdentifier = table.Column<string>(type: "text", nullable: true),
                    WaterBodyName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterStations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterRecords_StationId",
                table: "WaterRecords",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaterRecords_WaterStations_StationId",
                table: "WaterRecords",
                column: "StationId",
                principalTable: "WaterStations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaterRecords_WaterStations_StationId",
                table: "WaterRecords");

            migrationBuilder.DropTable(
                name: "WaterStations");

            migrationBuilder.DropIndex(
                name: "IX_WaterRecords_StationId",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "ParameterCode",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "SampleDate",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "WaterRecords");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "WaterRecords");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "WaterRecords",
                newName: "StdevValue");

            migrationBuilder.AlterColumn<string>(
                name: "Location_TzId",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location_Region",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location_Name",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Location_Lon",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Location_LocaltimeEpoch",
                table: "WeatherRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location_Localtime",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Location_Lat",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location_Country",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_WindMph",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_WindKph",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Current_WindDir",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_WindDegree",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_Uv",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_TempF",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_TempC",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_PressureMb",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_PressureIn",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_PrecipMm",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_PrecipIn",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Current_LastUpdatedEpoch",
                table: "WeatherRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Current_LastUpdated",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_IsDay",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_Humidity",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_FeelslikeF",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_FeelslikeC",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Current_Condition_Text",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Current_Condition_Icon",
                table: "WeatherRecords",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_Condition_Code",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_Cloud",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_AirQuality_UsEpaIndex",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_So2",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_Pm25",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_Pm10",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_O3",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_No2",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Current_AirQuality_GbDefraIndex",
                table: "WeatherRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Current_AirQuality_Co",
                table: "WeatherRecords",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class1",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Class2",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Class3",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Class4",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Class5",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryGroup",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EeaIndicator",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Lat",
                table: "WaterRecords",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Lon",
                table: "WaterRecords",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxValue",
                table: "WaterRecords",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MeanValue",
                table: "WaterRecords",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinValue",
                table: "WaterRecords",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfReportedSites",
                table: "WaterRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSites",
                table: "WaterRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhenomenonTimeReferenceYear",
                table: "WaterRecords",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultUom",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WaterBodyCategory",
                table: "WaterRecords",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
