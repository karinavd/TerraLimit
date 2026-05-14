using DbUpdater.Models;
using Microsoft.EntityFrameworkCore;

namespace DbUpdater.Data
{
    public class EcoDb : DbContext
    {
        public EcoDb(DbContextOptions<EcoDb> options) : base(options)
        {

        }

        public DbSet<WeatherRecord> WeatherRecords { get; set; }
        public DbSet<WaterRecord> WaterRecords { get; set; }
        public DbSet<WaterStation> WaterStations { get; set; }
    }
}