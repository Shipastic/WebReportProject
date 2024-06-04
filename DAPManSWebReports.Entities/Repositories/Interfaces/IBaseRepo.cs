namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        IEnumerable<T> ReadListByParentID(int parentID);
        Task<IEnumerable<T>> ReadListParentEntities();
        void Delete(int id);
        Task<T> Create(T obj);
        Task<T> ReadById(int id);
        void Update(T obj);
    }
}
