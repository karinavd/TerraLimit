using Microsoft.EntityFrameworkCore;
using TerraLimit.Model.Entities;
using TerraLimit.Model.Interfaces;
using TerraLimit.Persistence.Data;

namespace TerraLimit.Persistence.Repositories
{

    public class WaterReadRepository : BaseReadRepository, IWaterReadRepository
    {
        public WaterReadRepository(EcoState dbContext) : base(dbContext)
        {
        }

        public async Task<List<WaterRecord>> GetRecordsByStationAsync(string stationId)
        {
            return await _dbContext.WaterRecords
                .Include(r => r.ParameterCodeNavigation)
                .AsNoTracking()
                .Where(r => r.StationId == stationId)
                .ToListAsync();
        }
    }
}