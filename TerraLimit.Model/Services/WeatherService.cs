using TerraLimit.Model.Interfaces;

namespace TerraLimit.Model.Services
{
    public class WeatherService : BaseService, IWeatherService
    {
        public WeatherService(IBaseReadRepository repository) : base(repository)
        {
        }
    }
}