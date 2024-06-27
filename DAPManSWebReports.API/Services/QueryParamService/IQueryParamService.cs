namespace DAPManSWebReports.API.Services.QueryParamService
{
    public interface IQueryParamService<T> where T : class
    {
        T GetQueryStringParam(HttpContext context);
        Dictionary<string, object> GetDictionaryFromQueryString(T obj);
    }
}
