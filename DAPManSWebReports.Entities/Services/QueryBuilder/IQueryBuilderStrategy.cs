using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Services.QueryBuilder
{
    public interface IQueryBuilderStrategy
    {
        string BuildQuery();
        Task<DataTable> ExecuteQuery(string query);
    }
}
