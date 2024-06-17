using DAPManSWebReports.Entities.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface IQueryRepo<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<T> ReadById(int id, int limit, int offset);
        Task<T> ReadById(int dataviewId, Dictionary<string, object> queryparams);
        //Task<int> GetCountById(int dataviewId, Dictionary<string, object> queryparams, string queryString, int dataSourceId);
        //T1 ExecuteById(int dataViewId);
    }
}
