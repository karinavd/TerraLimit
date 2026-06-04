namespace TerraLimit.Model.Interfaces
{
    public interface IBaseService
    {
        Task<TDto?> GetOneRecordAsync<TEntity, TDto, TKey>(TKey id)
            where TEntity : class, IEntity<TKey> where TDto : new();
        Task<List<TDto>> GetPageAsync<TEntity, TDto, TKey>(int offset, int limit)
            where TEntity : class, IEntity<TKey> where TDto : new();
    }
}