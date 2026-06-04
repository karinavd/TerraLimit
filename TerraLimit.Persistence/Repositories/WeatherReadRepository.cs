using TerraLimit.Model.Interfaces;
using TerraLimit.Persistence.Data;

namespace TerraLimit.Persistence.Repositories
{
    public class WeatherReadRepository : BaseReadRepository, IWeatherReadRepository
    {
        public WeatherReadRepository(EcoState dbContext) : base(dbContext)
        {
        }
    }
}