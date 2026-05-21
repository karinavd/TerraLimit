using Microsoft.EntityFrameworkCore;
using TerraLimit.Api.Data;
using TerraLimit.Api.DTOs;
using TerraLimit.Api.Extensions;
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

        public async Task<List<WeatherLocation>> GetAllWeatherLocationsAsync(int offset, int limit)
        {
            return await _dbContext.WeatherLocations
                .AsNoTracking()
                .OrderBy(l => l.LocationId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<WeatherLocation?> GetWeatherLocationAsync(int id)
        {
            return await _dbContext.WeatherLocations.FindAsync(id);
        }

        public async Task<List<WeatherObservationDTO>> GetAllWeatherObservationsAsync(int offset, int limit)
        {
            var observations = await _dbContext.WeatherObservations
                .AsNoTracking()
                .OrderBy(l => l.LocationId)
                .Select(o => o)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return [.. observations.Select(o => o.ToDTO())];
        }

        public async Task<WeatherObservationDTO?> GetWeatherObservationAsync(int id)
        {
            return (await _dbContext.WeatherObservations.FindAsync(id))?.ToDTO();
        }
    }
}