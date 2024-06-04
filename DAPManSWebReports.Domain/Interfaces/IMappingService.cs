namespace DAPManSWebReports.Domain.Interfaces
{
    public interface IMappingService<T> where T : class
    {
        Task<IEnumerable<T>> GetDtoList();

    }
}