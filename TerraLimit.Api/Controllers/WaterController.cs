using Microsoft.AspNetCore.Mvc;
using TerraLimit.Model.Contracts;
using TerraLimit.Model.Interfaces;
using TerraLimit.Model.Entities;

namespace TerraLimit.Api.Host.Controllers
{
    [ApiController]
    [Route("water")]
    public class WaterController : ControllerBase
    {
        private readonly IWaterService _service;

        public WaterController(IWaterService service)
        {
            _service = service;
        }

        [HttpGet("records")]
        public async Task<ActionResult<List<WaterRecordDto>>> GetRecords([FromQuery] int offset = 0, [FromQuery] int limit = 100)
        {
            var pageItems = await _service.GetPageAsync<WaterRecord, WaterRecordDto, int>(offset, limit);
            return pageItems?.Count == 0 ? NotFound("Page is empty") : Ok(pageItems);
        }

        [HttpGet("records/{id}")]
        public async Task<ActionResult<WaterRecordDto>> GetRecord([FromRoute] int id)
        {
            var record = await _service.GetOneRecordAsync<WaterRecord, WaterRecordDto, int>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("parameters")]
        public async Task<ActionResult<List<WaterParameterDto>>> GetParameters([FromQuery] int offset = 0, [FromQuery] int limit = 30000)
        {
            var pageItems = await _service.GetPageAsync<WaterParameter, WaterParameterDto, string>(offset, limit);
            return pageItems?.Count == 0 ? NotFound("Page is empty") : Ok(pageItems);
        }

        [HttpGet("parameters/{id}")]
        public async Task<ActionResult<WaterParameterDto>> GetParameter([FromRoute] string id)
        {
            var record = await _service.GetOneRecordAsync<WaterParameter, WaterParameterDto, string>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("stations")]
        public async Task<ActionResult<List<WaterStationDto>>> GetStations([FromQuery] int offset = 0, [FromQuery] int limit = 100)
        {
            var pageItems = await _service.GetPageAsync<WaterStation, WaterStationDto, string>(offset, limit);
            return pageItems?.Count == 0 ? NotFound("Page is empty") : Ok(pageItems);
        }

        [HttpGet("stations/{id}")]
        public async Task<ActionResult<WaterStationDto>> GetStation([FromRoute] string id)
        {
            var record = await _service.GetOneRecordAsync<WaterStation, WaterStationDto, string>(id);
            return record is null ? NotFound("Record not found") : Ok(record);
        }

        [HttpGet("stations/{stationId}/records")]
        public async Task<ActionResult<List<WaterRecordDto>>> GetRecordsForStation([FromRoute] string stationId)
        {
            var records = await _service.GetRecordsByStationAsync(stationId);

            if (records == null || records.Count == 0)
            {
                return NotFound($"No records found for station: {stationId}");
            }

            return Ok(records);
        }
    }
}