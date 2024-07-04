using DAPManSWebReports.Entities.Constants;
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

using Oracle.ManagedDataAccess.Client;

using System.Data;
using System.Data.Common;

using DataView = DAPManSWebReports.Entities.Models.DataView;

namespace DAPManSWebReports.Entities.Services.QueryBuilder
{
    public class QueryBuilderIntel : IQueryBuilderStrategy
    {
        private string _baseQuery;
        private DataView _dv;
        private readonly DatabaseConnection _conn;
        private readonly IDatabaseConnection _dbConnection;
        private List<DbParameter> _parameters;
        public QueryBuilderIntel(DataView dv, string dbType, string dbString)
        {
            _dv = dv;
            _baseQuery = _dv.Query;          
            _conn = new DatabaseConnection(dbType, dbString);
            _parameters = new List<DbParameter>();
        }
        private bool CheckQuery()
        {
            string str = _baseQuery.ToUpper().Trim();
            return str.IndexOf("SELECT") == 0 || str.IndexOf("WITH") == 0;
        }
        private string ConvertDateTime(string paramName, DateTime date)
        {

            if (_conn.DbSourceType.ToString().ToLower().Equals("oracle"))
            {
                _conn.AddParameter(paramName, date.ToString("yyyy-MM-dd HH:mm:ss"), DbType.DateTime);
                return date.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (_conn.DbSourceType.ToString().ToLower().Equals("sqlite"))
            {
                _conn.AddParameter(paramName, date.Year + date.Month + date.Day, DbType.Int32);
                return "?";
            }
            if (!_conn.DbSourceType.ToString().ToLower().Equals("sqlserver"))
                return "";
            _conn.AddParameter(paramName, string.Format($"{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}"), DbType.String);
            return "CONVERT(datetime, ?, 120)";
        }
        private void ReplaceParameters(ref string sqlQuery, DateTime fromDate, DateTime toDate)
        {
            foreach (SpParameter parameter in _dv.parameters)
            {
                if (parameter.Name.ToUpper() == "STARTDATE")
                {
                    string newValue = ConvertDateTime("startDate", fromDate);
                    sqlQuery = sqlQuery.Replace("?" + parameter.Name + ";", newValue);
                }
                else if (parameter.Name.ToUpper() == "STOPDATE")
                {
                    string newValue = ConvertDateTime("stopDate", toDate);
                    sqlQuery = sqlQuery.Replace("?" + parameter.Name + ";", newValue);
                }
                else if (parameter.Name != "")
                {
                    sqlQuery = sqlQuery.Replace("?" + parameter.Name + ";", "?");
                    _conn.AddParameter("@" + parameter.Name, parameter.Value, DbType.String);
                }
            }
        }
        private void SetAdditionalQuery(ref string sqlQuery, DateTime fromDate, DateTime toDate)
        {
            string str1 = "";
            string upper = sqlQuery.ToUpper();
            int num1 = upper.IndexOf("ORDER BY");
            int num2 = upper.IndexOf("GROUP BY");
            int num3 = upper.IndexOf("WHERE");
            if (num2 > -1 && num1 > -1)
            {
                str1 = sqlQuery.Substring(num2);
                sqlQuery = sqlQuery.Substring(0, num2);
            }
            else if (num1 > -1)
            {
                str1 = sqlQuery.Substring(num1);
                sqlQuery = sqlQuery.Substring(0, num1);
            }
            if (_dv.StartDateField != "")
            {
                string str2 = ConvertDateTime("startDate", fromDate);
                if (num3 > -1)
                {
                    sqlQuery = $"{sqlQuery} AND {_dv.StartDateField} >= TO_DATE({str2}, 'YYYY-MM-DD HH24:MI:SS')";
                }
                else
                {
                    sqlQuery = $"{sqlQuery} WHERE {_dv.StartDateField} >= TO_DATE({str2}, 'YYYY-MM-DD HH24:MI:SS')";
                    num3 = sqlQuery.IndexOf("WHERE");
                }
            }
            if (_dv.StopDateField != "")
            {
                string str3 = ConvertDateTime("stopDate", toDate);
                sqlQuery = num3 <= -1 ? $"{sqlQuery} WHERE {_dv.StartDateField} <= TO_DATE({str3}, 'YYYY-MM-DD HH24:MI:SS')" : $"{sqlQuery} AND {_dv.StartDateField} <= TO_DATE({str3}, 'YYYY-MM-DD HH24:MI:SS')";
            }
            sqlQuery = sqlQuery + " " + str1;
        }
        public string BuildQuery()
        {
            if (!CheckQuery())
                return ReportMessageCodes.CheckQuery.ID.ToString();
            _baseQuery = _dv.Query;
            ReplaceParameters(ref _baseQuery, _dv.StartDate, _dv.StopDate);
            SetAdditionalQuery(ref _baseQuery, _dv.StartDate, _dv.StopDate);
            _baseQuery = _baseQuery.Replace('\n', ' ');
            _baseQuery = _baseQuery.Replace('\r', ' ');
            return _baseQuery;
        }
        public async Task<DataTable> ExecuteQuery(string query)
        {
           return await _conn.ExecuteQuery(query);
        }

        public IEnumerable<DbParameter> GetParameters()
        {
            return _parameters;
        }
    }
}
