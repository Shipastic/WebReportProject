using DAPManSWebReports.Infrastructure.Interfaces;
using LoggingLibrary.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class PostgreSqlDbBuilder : BaseConBuilder
    {
        private IDataSource _dataSource;
        private IConfiguration _configuration;
        public PostgreSqlDbBuilder(IDataSource dataSource, IConfiguration configuration, ILogger<BaseConBuilder> logger) : base(configuration, logger)
        {
            _configuration = configuration;
            _dataSource = dataSource;
        }
    }
}
