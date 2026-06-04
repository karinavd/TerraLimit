using TerraLimit.Model.Interfaces;
using TerraLimit.Model.Mapping;

namespace TerraLimit.Model.Services
{
    public class BaseService : IBaseService
    {
        private readonly IBaseReadRepository _repository;

        public BaseService(IBaseReadRepository repository)
        {
            _repository = repository;
        }

        public async Task<TDto?> GetOneRecordAsync<TEntity, TDto, TKey>(TKey id)
            where TEntity : class, IEntity<TKey> where TDto : new()
        {
            if (id is int intId && intId <= 0) return default;

            var entity = await _repository.GetByIdAsync<TEntity, TKey>(id);
            return entity == null ? default : MyMapper.MapTo<TDto>(entity);
        }

        public async Task<List<TDto>> GetPageAsync<TEntity, TDto, TKey>(int offset, int limit)
            where TEntity : class, IEntity<TKey> where TDto : new()
        {
            if (offset < 0) offset = 0;
            if (limit > 5000) limit = 5000;

            var entities = await _repository.GetPageAsync<TEntity, TKey>(offset, limit);
            return [.. entities.Select(MyMapper.MapTo<TDto>)];
        }
    }
}