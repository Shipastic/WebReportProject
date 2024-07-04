using Microsoft.Extensions.Logging;

using Oracle.ManagedDataAccess.Client;

using System.Data;

namespace DAPManSWebReports.Entities.Services.QueryBuilder
{
    public class QueryBuilder : IQueryBuilderStrategy
    {
        private string _baseQuery;
        private List<string> _additionalConditions;
        private string _paginationClause;
        private List<OracleParameter> _parameters;
        private Models.DataView _dv;
        private string oracleConnectionString;

        public QueryBuilder(Models.DataView dv, string connectionOracleDbString)
        {
            _dv = dv;
            _baseQuery = _dv.Query;
            _additionalConditions = new List<string>();
            _paginationClause = "";
            _parameters = new List<OracleParameter>();
            oracleConnectionString = connectionOracleDbString;
        }

        public QueryBuilder AddDateFilter(string startDateField, string stopDateField, DateTime startDate, DateTime stopDate)
        {
            if (   !string.IsNullOrEmpty(_dv.StartDateField) 
                && !string.IsNullOrEmpty(_dv.StopDateField)
                && !string.IsNullOrEmpty(Convert.ToString(_dv.StartDate))
                && !string.IsNullOrEmpty(Convert.ToString(_dv.StopDate)))
            {
                string condition = $"{startDateField} >= TO_DATE(:startDate, 'YYYY-MM-DD HH24:MI:SS') AND {stopDateField} <= TO_DATE(:stopDate, 'YYYY-MM-DD HH24:MI:SS')";
                _additionalConditions.Add(condition);

                _parameters.Add(new OracleParameter("startDate", startDate.ToString("yyyy-MM-dd HH:mm:ss")));
                _parameters.Add(new OracleParameter("stopDate", stopDate.ToString("yyyy-MM-dd HH:mm:ss")));     
            }
            return this;
        }

        //public QueryBuilder AddOffsetLimit(int offset, int limit)
        //{
        //    _paginationClause = $" OFFSET :offset ROWS FETCH NEXT :limit ROWS ONLY";
        //    _parameters.Add(new OracleParameter("offset", offset));
        //    _parameters.Add(new OracleParameter("limit", limit));
        //    return this;
        //}

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

        public async Task<DataTable> ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            using (OracleConnection con = new OracleConnection(oracleConnectionString))
            {
                try
                {
                    await con.OpenAsync();
                    using (OracleCommand cmd = con.CreateCommand())
                    {

                        cmd.CommandText = query;
                        Console.WriteLine($"{DateTime.Now}|\t  Create Query to db: {query}");

                        var parameters = GetParameters();
                        foreach (var parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }

                        using var adapter = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
                        try
                        {
                            adapter.Fill(dt);
                        }
                        catch (InvalidCastException ex)
                        {
                            Console.WriteLine($"{DateTime.Now}|\t An error occurred during mapping: {ex.Message}");

                            foreach (var er in dt.GetErrors())
                            {
                                Console.WriteLine($"{er.GetType()}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{DateTime.Now}|\t {ex.Message}\t{ex.InnerException}");
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.Now}|\t {ex.Message}\t{ex.InnerException}");
                    return null;
                }
                return dt;
            }
        }

    }
}
