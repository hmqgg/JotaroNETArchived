namespace Jotaro.Entity.Interfaces
{
    public interface IHasId<out TId>
    {
        TId Id { get; }
    }
}
