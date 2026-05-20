using TerraLimit.Api.Services;

namespace TerraLimit.Api.Endpoints
{
    public static class WeatherEndpoints
    {
        public static IEndpointRouteBuilder MapWeatherEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("weather");

            group.MapGet("/locations/{id}", GetWeatherLocationAsync);

            return group;
        }

        private static async Task<IResult> GetWeatherLocationAsync(WeatherService weatherService, int id)
        {
            var location = await weatherService.GetWeatherLocationAsync(id);
            return location is null ? TypedResults.NotFound("Location not found") : TypedResults.Ok(location);
        }
    }
}