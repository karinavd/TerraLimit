using TerraLimit.Api.DTOs;
using TerraLimit.Api.Interfaces;
using TerraLimit.Api.Models;
using TerraLimit.Api.Services;

namespace TerraLimit.Api.Endpoints
{
    public static class AllEndpoints
    {
        public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("weather");

            group.MapGet("/locations/{id}", GetOneRecordAsync<WeatherLocation, WeatherLocationDTO>);
            group.MapGet("/locations", GetPageAsync<WeatherLocation, WeatherLocationDTO>);

            group.MapGet("/observations/{id}", GetOneRecordAsync<WeatherObservation, WeatherObservationDTO>);
            group.MapGet("/observations", GetPageAsync<WeatherObservation, WeatherObservationDTO>);

            group.MapGet("/air_qualities/{id}", GetOneRecordAsync<AirQualityIndex, AirQualityIndexDTO>);
            group.MapGet("/air_qualities", GetPageAsync<AirQualityIndex, AirQualityIndexDTO>);

            group.MapGet("/atmosphere_metrics/{id}", GetOneRecordAsync<AtmosphereMetric, AtmosphereMetricDTO>);
            group.MapGet("/atmosphere_metrics", GetPageAsync<AtmosphereMetric, AtmosphereMetricDTO>);

            return group;
        }

        private static async Task<IResult> GetOneRecordAsync<TEntity, TDto>(EndpointService weatherService, int id)
                                                             where TEntity : class, IEntity<int>
                                                             where TDto : new()
        {
            var record = await weatherService.GetOneRecordAsync<TEntity, TDto>(id);
            return record is null ? TypedResults.NotFound("Record not found") : TypedResults.Ok(record);
        }

        private static async Task<IResult> GetPageAsync<TEntity, TDto>(EndpointService weatherService, int offset = 0, int limit = 100)
                                                        where TEntity : class, IEntity<int>
                                                        where TDto : new()
        {
            var pageItems = await weatherService.GetPageAsync<TEntity, TDto>(offset, limit);
            return pageItems?.Count == 0 ? TypedResults.NotFound("Page is empty") : TypedResults.Ok(pageItems);
        }
    }
}