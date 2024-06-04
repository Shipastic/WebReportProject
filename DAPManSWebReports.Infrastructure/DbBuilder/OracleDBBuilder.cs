using DAPManSWebReports.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class OracleDBBuilder : BaseConBuilder
    {
        private IDataSource _dataSource;

        private IConfiguration _configuration;
        public OracleDBBuilder(IDataSource dataSource, IConfiguration configuration) :base(configuration)
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
