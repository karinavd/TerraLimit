using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using TerraLimit.Api.Data;
using TerraLimit.Api.DTOs;
using TerraLimit.Api.Extensions;
using TerraLimit.Api.Interfaces;
using TerraLimit.Api.Models;

namespace TerraLimit.Api.Services
{
    class EndpointService
    {
        private readonly EcoState _dbContext;

        public EndpointService(EcoState dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<TDto>> GetPageAsync<TEntity, TDto>(int offset, int limit) where TEntity : class, IEntity<int>
                                                                                         where TDto : new()
        {
            var entities = await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return [.. entities.Select(MyMapper.MapTo<TDto>)];
        }

        public async Task<TDto?> GetOneRecordAsync<TEntity, TDto>(int id) where TEntity : class, IEntity<int>
                                                                          where TDto : new()
        {
            var record = await _dbContext.Set<TEntity>().FindAsync(id);
            return record is null ? default : MyMapper.MapTo<TDto>(record);
        }
    }
}