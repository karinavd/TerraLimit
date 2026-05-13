using Microsoft.EntityFrameworkCore;

public class EcoDb : DbContext
{
    public EcoDb(DbContextOptions<EcoDb> options) : base(options)
    {

    }
    public DbSet<WeatherRecord> WeatherRecords { get; set; }
}