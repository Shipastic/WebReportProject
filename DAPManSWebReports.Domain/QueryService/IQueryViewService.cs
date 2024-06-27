using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.QueryService
{
    public interface IQueryViewService<T> where T : class
    {
        Task<T> GetQueryView(int dataviewId, int limit, int offset);
        Task<T> GetQueryViewWithParam(int dataviewId, Dictionary<string, object> queryparams);
    }
}
