using Microsoft.EntityFrameworkCore;
using TerraLimit.Api.Data;
using TerraLimit.Api.Models;

namespace TerraLimit.Api.Services
{
    class WeatherService
    {
        private readonly EcoState _dbContext;
        public WeatherService(EcoState dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<WeatherLocation>?> GetAllWeatherLocationsAsync(int offset, int limit)
        {
            var orderedQuery = _dbContext.WeatherLocations.AsQueryable().OrderBy(l => l.LocationId);

            var paginatedQuery = orderedQuery.Skip(offset).Take(limit);

            return await paginatedQuery.ToListAsync();
        }

        public async Task<WeatherLocation?> GetWeatherLocationAsync(int id)
        {
            return await _dbContext.WeatherLocations.FindAsync(id);
        }
    }
}