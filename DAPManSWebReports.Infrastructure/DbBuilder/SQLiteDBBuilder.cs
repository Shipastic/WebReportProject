using DAPManSWebReports.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class SQLiteDBBuilder : BaseConBuilder
    {
        private IDataSource _dataSource;
        private IConfiguration _configuration;
        public SQLiteDBBuilder(IDataSource dataSource, IConfiguration configuration, ILogger<BaseConBuilder> logger) : base(configuration, logger)
        {
            _configuration = configuration;
            _dataSource = dataSource;
        }
    }
}
