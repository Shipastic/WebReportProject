using DAPManSWebReports.Infrastructure.Interfaces;
using LoggingLibrary.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class OracleDBBuilder : BaseConBuilder
    {
        private IDataSource _dataSource;

        private IConfiguration _configuration;

        public OracleDBBuilder(IDataSource dataSource, IConfiguration configuration, ILogger<BaseConBuilder> oracleLogger) : base(configuration, oracleLogger)
        {
            _configuration = configuration;
            _dataSource = dataSource;
        }    

        public string GetConnectionStringDb()
        {
            var dataBase = $"Data Source={_dataSource.Server}/{_dataSource.DataBase}";
            var user     = $"User ID={_dataSource.DbUser}";
            var password = $"Password={_dataSource.DbPassword}";
            
            string oracleConnString = string.Join(";", [dataBase, user, password]);

            return oracleConnString;           
        }
        
    }
}
