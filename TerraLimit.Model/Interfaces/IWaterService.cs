using TerraLimit.Model.Contracts;

namespace TerraLimit.Model.Interfaces
{
    public interface IWaterService : IBaseService
    {
        Task<List<WaterRecordDto>> GetRecordsByStationAsync(string stationId);
    }
}