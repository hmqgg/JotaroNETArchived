namespace Jotaro.Entity.Interfaces
{
    public interface IHasClassId<out TClassId> : IHasId<TClassId> where TClassId : class
    {
    }
}
