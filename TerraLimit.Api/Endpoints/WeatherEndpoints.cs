using TerraLimit.Api.Services;

namespace TerraLimit.Api.Endpoints
{
    public static class WeatherEndpoints
    {
        public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("weather");

            group.MapGet("/locations/{id}", GetWeatherLocationAsync);
            group.MapGet("/locations", GetAllWeatherLocationsAsync);

            return group;
        }

        private static async Task<IResult> GetWeatherLocationAsync(WeatherService weatherService, int id)
        {
            var location = await weatherService.GetWeatherLocationAsync(id);
            return location is null ? TypedResults.NotFound("Location not found") : TypedResults.Ok(location);
        }

        private static async Task<IResult> GetAllWeatherLocationsAsync(WeatherService weatherService, int offset = 0, int limit = 100)
        {
            var locations = await weatherService.GetAllWeatherLocationsAsync(offset, limit);
            return locations?.Count == 0 ? TypedResults.NotFound("Locations list is empty") : TypedResults.Ok(locations);
        }
    }
}