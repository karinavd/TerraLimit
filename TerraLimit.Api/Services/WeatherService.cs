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

        public async Task<WeatherLocation?> GetWeatherLocationAsync(int id)
        {
            return await _dbContext.WeatherLocations.FindAsync(id);
        }
    }
}