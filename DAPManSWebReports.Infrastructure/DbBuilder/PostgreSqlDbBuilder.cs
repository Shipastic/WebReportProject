using DAPManSWebReports.Infrastructure.Interfaces;
using DAPManSWebReports.Infrastructure.Models;

using Microsoft.Extensions.Configuration;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class PostgreSqlDbBuilder : BaseConBuilder
    {
        private IDataSource _dataSource;
        private IConfiguration _configuration;
        public PostgreSqlDbBuilder(IDataSource dataSource, IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _dataSource = dataSource;
        }
    }
}
