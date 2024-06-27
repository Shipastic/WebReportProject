namespace DAPManSWebReports.Domain.MappingService
{
    public interface IMappingService<T> where T : class
    {
        Task<IEnumerable<T>> GetDtoList();

    }
}