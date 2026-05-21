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

        public static AirQualityIndexDTO ToDTO(this AirQualityIndex aq)
        {
            return new AirQualityIndexDTO
            {
                RecordId = aq.RecordId,
                Co = aq.Co,
                No2 = aq.No2,
                O3 = aq.O3,
                Pm10 = aq.Pm10,
                Pm25 = aq.Pm25,
                So2 = aq.So2,
                GbDefraIndex = aq.GbDefraIndex,
                UsEpaIndex = aq.UsEpaIndex
            };
        }
    }
}