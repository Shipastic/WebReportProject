using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace DAPManSWebReports.Entities.Models
{
    public class QueryBuilder
    {
        private string _baseQuery;
        private string _whereClause;
        private string _paginationClause;
        private List<OracleParameter> _parameters;

        public QueryBuilder(string baseQuery)
        {
            _baseQuery = baseQuery;
            _whereClause = "";
            _paginationClause = "";
            _parameters = new List<OracleParameter>();
        }

        public QueryBuilder AddDateFilter(string startDateField, string stopDateField, DateTime startDate, DateTime endDate)
        {
            if (!string.IsNullOrEmpty(_whereClause))
            {
                _whereClause += " AND";
            }
            else
            {
                _whereClause = " WHERE";
            }
            _whereClause += $" {startDateField} >= TO_DATE(:startDate, 'YYYY-MM-DD HH24:MI:SS') AND {stopDateField} <= TO_DATE(:endDate, 'YYYY-MM-DD HH24:MI:SS')";
            _parameters.Add(new OracleParameter("startDate", startDate.ToString("yyyy-MM-dd HH:mm:ss")));
            _parameters.Add(new OracleParameter("endDate", endDate.ToString("yyyy-MM-dd HH:mm:ss")));
            return this;
        }

        public QueryBuilder AddOffsetLimit(int offset, int limit)
        {
            _paginationClause = $" OFFSET :offset ROWS FETCH NEXT :limit ROWS ONLY";
            _parameters.Add(new OracleParameter("offset", offset));
            _parameters.Add(new OracleParameter("limit", limit));
            return this;
        }

        public string BuildQuery()
        {
            // Проверка наличия ORDER BY
            int orderByIndex = _baseQuery.IndexOf(" order by", StringComparison.OrdinalIgnoreCase);
            string query;

            if (orderByIndex >= 0)
            {
                // Разделяем запрос на основную часть и часть с ORDER BY
                string mainQuery = _baseQuery.Substring(0, orderByIndex);
                string orderByClause = _baseQuery.Substring(orderByIndex);

                query = $"{mainQuery} {_whereClause} {orderByClause}{_paginationClause}";
            }
            else
            {
                query = $"{_baseQuery} {_whereClause}{_paginationClause}";
            }

            return query;
        }

        public IEnumerable<OracleParameter> GetParameters()
        {
            return _parameters;
        }
    }
}
