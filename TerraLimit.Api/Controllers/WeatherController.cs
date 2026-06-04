using Microsoft.AspNetCore.Mvc;
using TerraLimit.Model.Contracts;
using TerraLimit.Model.Interfaces;
using TerraLimit.Model.Entities;

namespace TerraLimit.Api.Host.Controllers
{
    [ApiController]
    [Route("weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        // GET: /weather/locations?offset=0&limit=100
        [HttpGet("locations")]
        public async Task<ActionResult<List<WeatherLocationDto>>> GetLocations([FromQuery] int offset = 0, [FromQuery] int limit = 100)
        {
            var pageItems = await _service.GetPageAsync<WeatherLocation, WeatherLocationDto, int>(offset, limit);
            return pageItems?.Count == 0 ? NotFound("Page is empty") : Ok(pageItems);
        }

        // GET: /weather/locations/{id}
        [HttpGet("locations/{id}")]
        public async Task<ActionResult<WeatherLocationDto>> GetLocation([FromRoute] int id)
        {
            var record = await _service.GetOneRecordAsync<WeatherLocation, WeatherLocationDto, int>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("observations")]
        public async Task<ActionResult<List<WeatherObservationDto>>> GetObservations([FromQuery] int offset = 0, [FromQuery] int limit = 100)
        {
            var pageItems = await _service.GetPageAsync<WeatherObservation, WeatherObservationDto, int>(offset, limit);
            return pageItems?.Count == 0 ? NotFound("Page is empty") : Ok(pageItems);
        }

        [HttpGet("observations/{id}")]
        public async Task<ActionResult<WeatherObservationDto>> GetObservation([FromRoute] int id)
        {
            var record = await _service.GetOneRecordAsync<WeatherObservation, WeatherObservationDto, int>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("air_qualities")]
        public async Task<ActionResult<AirQualityIndexDto>> GetAirQualities([FromQuery] int offset = 0, [FromQuery] int limit = 100)
        {
            var record = await _service.GetPageAsync<AirQualityIndex, AirQualityIndexDto, int>(offset, limit);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("air_qualities/{id}")]
        public async Task<ActionResult<AirQualityIndexDto>> GetAirQuality([FromRoute] int id)
        {
            var record = await _service.GetOneRecordAsync<AirQualityIndex, AirQualityIndexDto, int>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("atmosphere_metrics")]
        public async Task<ActionResult<AtmosphereMetricDto>> GetAtmosphereMetrics([FromQuery] int offset = 0, [FromQuery] int limit = 100)
        {
            var record = await _service.GetPageAsync<AtmosphereMetric, AtmosphereMetricDto, int>(offset, limit);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("atmosphere_metrics/{id}")]
        public async Task<ActionResult<AtmosphereMetricDto>> GetAtmosphereMetric([FromRoute] int id)
        {
            var record = await _service.GetOneRecordAsync<AtmosphereMetric, AtmosphereMetricDto, int>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }
    }
}