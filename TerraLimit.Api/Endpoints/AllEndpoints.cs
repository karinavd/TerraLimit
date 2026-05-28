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

            group.MapGet("/locations/{id}", GetOneRecordAsync<WeatherLocation, WeatherLocationDTO, int>);
            group.MapGet("/locations", GetPageAsync<WeatherLocation, WeatherLocationDTO, int>);

            group.MapGet("/observations/{id}", GetOneRecordAsync<WeatherObservation, WeatherObservationDTO, int>);
            group.MapGet("/observations", GetPageAsync<WeatherObservation, WeatherObservationDTO, int>);

            group.MapGet("/air_qualities/{id}", GetOneRecordAsync<AirQualityIndex, AirQualityIndexDTO, int>);
            group.MapGet("/air_qualities", GetPageAsync<AirQualityIndex, AirQualityIndexDTO, int>);

            group.MapGet("/atmosphere_metrics/{id}", GetOneRecordAsync<AtmosphereMetric, AtmosphereMetricDTO, int>);
            group.MapGet("/atmosphere_metrics", GetPageAsync<AtmosphereMetric, AtmosphereMetricDTO, int>);

            return group;
        }

        public static IEndpointRouteBuilder MapWaterEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("water");

            group.MapGet("/records/{id}", GetOneRecordAsync<WaterRecord, WaterRecordDTO, int>);
            group.MapGet("/records", GetPageAsync<WaterRecord, WaterRecordDTO, int>);

            group.MapGet("/parameters/{id}", GetOneRecordAsync<WaterParameter, WaterParameterDTO, string>);
            group.MapGet("/parameters", GetPageAsync<WaterParameter, WaterParameterDTO, string>);

            group.MapGet("/stations/{id}", GetOneRecordAsync<WaterStation, WaterStationDTO, string>);
            group.MapGet("/stations", GetPageAsync<WaterStation, WaterStationDTO, string>);

            return group;
        }

        private static async Task<IResult> GetOneRecordAsync<TEntity, TDto, TKey>(EndpointService weatherService, TKey id)
                                                             where TEntity : class, IEntity<TKey>
                                                             where TDto : new()
        {
            var record = await weatherService.GetOneRecordAsync<TEntity, TDto, TKey>(id);
            return record is null ? TypedResults.NotFound("Record not found") : TypedResults.Ok(record);
        }

        private static async Task<IResult> GetPageAsync<TEntity, TDto, TKey>(EndpointService weatherService, int offset = 0, int limit = 100)
                                                        where TEntity : class, IEntity<TKey>
                                                        where TDto : new()
        {
            var pageItems = await weatherService.GetPageAsync<TEntity, TDto, TKey>(offset, limit);
            return pageItems?.Count == 0 ? TypedResults.NotFound("Page is empty") : TypedResults.Ok(pageItems);
        }
    }
}