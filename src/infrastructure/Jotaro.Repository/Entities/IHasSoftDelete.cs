namespace Jotaro.Repository.Entities
{
    public interface IHasSoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
