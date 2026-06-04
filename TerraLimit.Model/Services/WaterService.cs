using TerraLimit.Model.Contracts;
using TerraLimit.Model.Interfaces;
using TerraLimit.Model.Mapping;

namespace TerraLimit.Model.Services
{
    public class WaterService : BaseService, IWaterService
    {
        private readonly IWaterReadRepository _repository;

        public WaterService(IWaterReadRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task<List<WaterRecordDto>> GetRecordsByStationAsync(string stationId)
        {
            if (string.IsNullOrWhiteSpace(stationId))
            {
                return [];
            }

            var records = await _repository.GetRecordsByStationAsync(stationId);

            return [.. records.Select(r =>
            {
                var dto = MyMapper.MapTo<WaterRecordDto>(r);

                dto.ParameterName = r.ParameterCodeNavigation?.Name;
                dto.ParameterDescription = r.ParameterCodeNavigation?.Description;

                return dto;
            })];
        }
    }
}