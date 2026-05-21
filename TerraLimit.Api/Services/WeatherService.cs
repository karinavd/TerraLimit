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
            return await _dbContext.WeatherObservations
                .AsNoTracking()
                .OrderBy(l => l.LocationId)
                .Select(o => o.ToDTO())
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<WeatherObservationDTO?> GetWeatherObservationAsync(int id)
        {
            return (await _dbContext.WeatherObservations.FindAsync(id))?.ToDTO();
        }

        public async Task<List<AirQualityIndexDTO>> GetAllAirQualityAsync(int offset, int limit)
        {
            var aqs = await _dbContext.AirQualityIndexes
                .AsNoTracking()
                .OrderBy(l => l.RecordId)
                .Select(o => o)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return [.. aqs.Select(aq => aq.ToDTO())];
        }

        public async Task<AirQualityIndexDTO?> GetAirQualityAsync(int id)
        {
            return (await _dbContext.AirQualityIndexes.FindAsync(id))?.ToDTO();
        }

        public async Task<List<AtmosphereMetricDTO>> GetAllAtmosphereMetricsAsync(int offset, int limit)
        {
            var ams = await _dbContext.AtmosphereMetrics
                .AsNoTracking()
                .OrderBy(l => l.RecordId)
                .Select(o => o)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return [.. ams.Select(am => am.ToDTO())];
        }

        public async Task<AtmosphereMetricDTO?> GetAtmosphereMetricAsync(int id)
        {
            return (await _dbContext.AtmosphereMetrics.FindAsync(id))?.ToDTO();
        }
    }
}