using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class AddWaterDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryGroup = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    WaterBodyCategory = table.Column<string>(type: "text", nullable: false),
                    EeaIndicator = table.Column<string>(type: "text", nullable: false),
                    PhenomenonTimeReferenceYear = table.Column<int>(type: "integer", nullable: true),
                    ResultUom = table.Column<string>(type: "text", nullable: false),
                    MeanValue = table.Column<double>(type: "double precision", nullable: true),
                    StdevValue = table.Column<double>(type: "double precision", nullable: true),
                    MinValue = table.Column<double>(type: "double precision", nullable: true),
                    MaxValue = table.Column<double>(type: "double precision", nullable: true),
                    Class1 = table.Column<string>(type: "text", nullable: false),
                    Class2 = table.Column<string>(type: "text", nullable: false),
                    Class3 = table.Column<string>(type: "text", nullable: false),
                    Class4 = table.Column<string>(type: "text", nullable: false),
                    Class5 = table.Column<string>(type: "text", nullable: false),
                    NumberOfSites = table.Column<int>(type: "integer", nullable: true),
                    NumberOfReportedSites = table.Column<int>(type: "integer", nullable: true),
                    Lat = table.Column<double>(type: "double precision", nullable: true),
                    Lon = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterRecords");
        }
    }
}
