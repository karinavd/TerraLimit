using TerraLimit.Api.DTOs;
using TerraLimit.Api.Models;

namespace TerraLimit.Api.Extensions
{
    public static class WeatherExtensions
    {
        public static WeatherObservationDTO ToDTO(this WeatherObservation wo)
        {
            return new WeatherObservationDTO
            {
                RecordId = wo.RecordId,
                LocationId = wo.LocationId,
                Localtime = wo.Localtime,
                ConditionText = wo.ConditionText,
                CurrentConditionIconUrl = wo.CurrentConditionIcon
            };
        }
    }
}