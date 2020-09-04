namespace Jotaro.Repository.Entities
{
    public interface IHasClassId<out TClassId> : IHasId<TClassId> where TClassId : class
    {
    }
}
