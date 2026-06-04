using Microsoft.EntityFrameworkCore;
using TerraLimit.Model.Interfaces;
using TerraLimit.Persistence.Data;

namespace TerraLimit.Persistence.Repositories
{
    public class BaseReadRepository : IBaseReadRepository
    {
        protected readonly EcoState _dbContext;

        public BaseReadRepository(EcoState dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity?> GetByIdAsync<TEntity, TKey>(TKey id) where TEntity : class, IEntity<TKey>
        {
            return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id!.Equals(id));
        }

        public async Task<List<TEntity>> GetPageAsync<TEntity, TKey>(int offset, int limit) where TEntity : class, IEntity<TKey>
        {
            return await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}