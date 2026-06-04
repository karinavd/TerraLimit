using TerraLimit.Model.Entities;

namespace TerraLimit.Model.Interfaces
{
    public interface IWaterReadRepository : IBaseReadRepository
    {
        Task<List<WaterRecord>> GetRecordsByStationAsync(string stationId);
    }
}