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
        private List<string> _additionalConditions;
        private string _paginationClause;
        private List<OracleParameter> _parameters;

        public QueryBuilder(string baseQuery)
        {
            _baseQuery = baseQuery;
            _additionalConditions = new List<string>();
            _paginationClause = "";
            _parameters = new List<OracleParameter>();
        }

        public QueryBuilder AddDateFilter(string startDateField, string stopDateField, DateTime startDate, DateTime endDate)
        {
            string condition = $"{startDateField} >= TO_DATE(:startDate, 'YYYY-MM-DD HH24:MI:SS') AND {stopDateField} <= TO_DATE(:endDate, 'YYYY-MM-DD HH24:MI:SS')";
            _additionalConditions.Add(condition);

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
            string mainQuery = _baseQuery;

            // Собираем итоговый WHERE из базового запроса и новых условий
            string finalWhereClause = string.Join(" AND ", _additionalConditions);
            // Вставляем финальное условие WHERE
            if (!string.IsNullOrEmpty(finalWhereClause))
            {
                int lastWhereIndex = mainQuery.LastIndexOf("where", StringComparison.OrdinalIgnoreCase);
                if (lastWhereIndex != -1)
                {
                    // Подразумеваем, что базовый запрос уже имеет WHERE
                    int insertPosition = lastWhereIndex + "where".Length;
                    mainQuery = mainQuery.Insert(insertPosition, $" {finalWhereClause} AND ");
                }
                else
                {
                    // Базовый запрос не имеет WHERE
                    int orderByIndex = mainQuery.IndexOf("order by", StringComparison.OrdinalIgnoreCase);
                    if (orderByIndex != -1)
                    {
                        mainQuery = mainQuery.Insert(orderByIndex, $" WHERE {finalWhereClause}");
                    }
                    else
                    {
                        mainQuery += $" WHERE {finalWhereClause}";
                    }
                }
            }

            // Добавляем пагинацию, если есть
            if (!string.IsNullOrEmpty(_paginationClause))
            {
                mainQuery += _paginationClause;
            }

            return mainQuery;
        }

        public IEnumerable<OracleParameter> GetParameters()
        {
            return _parameters;
        }
    }
}
