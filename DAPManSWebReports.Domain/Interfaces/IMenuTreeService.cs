namespace DAPManSWebReports.Domain.Interfaces
{
    public interface IMenuTreeService<T> where T : class
    {
        Task<IEnumerable<T>> GetParentDtos();
        IEnumerable<T> GetChildDtos(int parentId);
        Task<T> GetDtoById(int id);
        Task<bool> UpdateDataAsync(T obj);
    }
}
