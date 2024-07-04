using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Services.QueryBuilder
{
    public class QueryBuilderContext
    {
        private IQueryBuilderStrategy _queryBuilderStrategy;
        public void SetQueryBuilderStrategy(IQueryBuilderStrategy strategy)
        {
            _queryBuilderStrategy = strategy;
        }
        public string BuildQuery()
        {
            return _queryBuilderStrategy.BuildQuery();
        }
        public Task<DataTable> ExecuteQuery(string query)
        {
            return  _queryBuilderStrategy.ExecuteQuery(query);
        }
    }
}
