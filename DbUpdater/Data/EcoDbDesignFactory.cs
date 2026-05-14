using DbUpdater.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

class DbDesignFactory : IDesignTimeDbContextFactory<EcoDb>
{
    public EcoDb CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

        var builder = new DbContextOptionsBuilder<EcoDb>();
        var connectionString = configuration.GetConnectionString("Default");

        builder.UseNpgsql(connectionString);

        return new EcoDb(builder.Options);
    }
}