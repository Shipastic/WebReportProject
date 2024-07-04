using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Infrastructure.Interfaces
{
    public interface IDatabaseConnection
    {
        void AddParameter(string name, object value, DbType type);
        Task<DataTable> ExecuteQuery(string query);
    }
}
