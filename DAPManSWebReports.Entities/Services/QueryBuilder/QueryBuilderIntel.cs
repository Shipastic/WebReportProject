using DAPManSWebReports.Entities.Constants;
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

using System.Data;
using System.Data.Common;

using DataView = DAPManSWebReports.Entities.Models.DataView;

namespace DAPManSWebReports.Entities.Services.QueryBuilder
{
    public class QueryBuilderIntel : IQueryBuilderStrategy
    {
        private string _baseQuery;
        private DataView _dv;
        private DatabaseConnection _conn;
        private readonly IDatabaseConnection _dbConnection;
        private List<DbParameter> _parameters;
        
        private DateTime _starDate;
        private DateTime _stopDate;
        public QueryBuilderIntel(DataView dv, string dbType, string dbString, DateTime StartDate, DateTime StopDate)
        {
            _dv         = dv;
            _baseQuery  = _dv.Query;          
            _conn       = new DatabaseConnection(dbType, dbString);
            _parameters = new List<DbParameter>();
            
            _starDate   = StartDate;
            _stopDate   = StopDate; 
        }
        private bool CheckQuery()
        {
            string str  = _baseQuery.ToUpper().Trim();
            return str.IndexOf("SELECT") == 0 || str.IndexOf("WITH") == 0;
        }
        private string ConvertDateTime(string paramName, DateTime date)
        {
            if (_conn.DbSourceType == DatabaseType.oracle)
            {
                _conn.AddParameter(paramName, date, DbType.DateTime);
                return $"?";
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
                    sqlQuery = sqlQuery.Replace("?" + parameter.Name + ";", ":startDate");
                    _conn.AddParameter("startDate", fromDate, DbType.DateTime);
                }
                else if (parameter.Name.ToUpper() == "STOPDATE")
                {
                    sqlQuery = sqlQuery.Replace("?" + parameter.Name + ";", ":stopDate");
                    _conn.AddParameter("stopDate", toDate, DbType.DateTime);
                }
                else if (parameter.Name.ToUpper() != "")
                {
                    string paramName = ":" + parameter.Name;
                }
            }
        }
        private void SetAdditionalQuery(ref string sqlQuery, DateTime fromDate, DateTime toDate)
        {
            string str1 = "";
            string upper = sqlQuery.ToUpper();
            int num1 = upper.IndexOf("ORDER BY");
            int num2 = upper.IndexOf("GROUP BY");
            int num3 = upper.IndexOf("WHERE"   );

            if (num2 > -1 && num1 > -1)
            {
                str1     = sqlQuery.Substring(num2);
                sqlQuery = sqlQuery.Substring(0, num2);
            }
            else if (num1 > -1)
            {
                str1     = sqlQuery.Substring(num1);
                sqlQuery = sqlQuery.Substring(0, num1);
            }
            if (_dv.StartDateField != "")
            {
                if (num3 > -1)
                {
                    sqlQuery = $"{sqlQuery} AND {_dv.StartDateField} >= :startDate";
                }
                else
                {
                    sqlQuery = $"{sqlQuery} WHERE {_dv.StartDateField} >= :startDate";
                    num3     = sqlQuery.IndexOf("WHERE");
                }
            }
            if (_dv.StopDateField != "")
            {
                sqlQuery = num3 <= -1 ? $"{sqlQuery} WHERE {_dv.StartDateField} <= :stopDate" : $"{sqlQuery} AND {_dv.StartDateField} <= :stopDate";
            }
            sqlQuery = sqlQuery + " " + str1;
        }
        public string BuildQuery()
        {
            if (!CheckQuery())
                return ReportMessageCodes.CheckQuery.ID.ToString();

            _baseQuery = _dv.Query;

            ReplaceParameters(ref _baseQuery,  _starDate, _stopDate);

            SetAdditionalQuery(ref _baseQuery, _starDate, _stopDate);

            return _baseQuery;
        }
        public async Task<DataTable> ExecuteQuery(string query)
        {        
            return await _conn.ExecuteQuery(query);
        }
    }
}
