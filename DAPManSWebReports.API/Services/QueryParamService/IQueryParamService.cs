using DAPManSWebReports.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.API.Services.QueryParamService
{
    public interface IQueryParamService<T> where T : class
    {
        T GetQueryStringParam(HttpContext context);
        Dictionary<string, object> GetDictionaryFromQueryString(T obj);
    }
}
