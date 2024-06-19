namespace DAPManSWebReports.API.Services.Sorting
{
    public interface ISortingService
    {
        IQueryable<T> ApplySorting<T>(IQueryable<T> source, string sortColumn, string sortType);
    }
}
