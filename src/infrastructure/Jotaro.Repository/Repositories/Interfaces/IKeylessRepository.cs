namespace Jotaro.Repository.Repositories.Interfaces
{
    public interface IKeylessRepository<T> : ICreateRepository<T>, IQueryRepository<T>, IRemoveRepository<T>,
        IUpdateByRepository<T> where T : class
    {
    }
}
