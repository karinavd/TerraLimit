namespace TerraLimit.Model.Interfaces
{
    public interface IBaseReadRepository
    {
        Task<TEntity?> GetByIdAsync<TEntity, TKey>(TKey id)
            where TEntity : class, IEntity<TKey>;
        Task<List<TEntity>> GetPageAsync<TEntity, TKey>(int offset, int limit)
            where TEntity : class, IEntity<TKey>;
    }
}