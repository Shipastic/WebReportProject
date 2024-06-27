namespace DAPManSWebReports.Domain.DataViewService
{
    public interface IDataViewService<T> where T : class
    {
        Task<IEnumerable<T>> GetParentDtosFromList(List<int> ids);
    }
}
