namespace Jotaro.Repository.Entities
{
    public interface IHasId<out TId>
    {
        TId Id { get; }
    }
}
