namespace TerraLimit.Api.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}