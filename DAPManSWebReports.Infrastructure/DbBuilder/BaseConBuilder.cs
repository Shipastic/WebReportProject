using DAPManSWebReports.Infrastructure.Interfaces;
using DAPManSWebReports.Infrastructure.Models;

using Microsoft.Extensions.Configuration;

using System.Data.SQLite;
using Microsoft.Extensions.Logging;
using LoggingLibrary.Service;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class BaseConBuilder : IBaseConBuilder
    {
        private readonly string _connectionString;

        private IConfiguration _configuration;

        protected readonly ILogger<BaseConBuilder> _logger;

        public BaseConBuilder(IConfiguration configuration, ILogger<BaseConBuilder> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _connectionString = _configuration.GetConnectionString("LocalConnection")
                            ?? throw new ArgumentException("Connection string cannot be null or empty", nameof(configuration));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _logger.LogInformation($"{DateTime.Now}|\t BaseConBuilder initialized with connection string:{_connectionString}");
        }
        private async Task<string> GetTypeDb(int datasourceId)
        {
            if (datasourceId <= 0)
            {
                _logger.LogError($"{DateTime.Now}|\t Invalid datasource ID: {datasourceId}" );
                throw new ArgumentException("Datasource ID must be greater than zero", nameof(datasourceId));
            }
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT lower(type) FROM DATASOURCE WHERE id=$datasourceId";
                    command.Parameters.AddWithValue("$datasourceId", datasourceId);
                    _logger.LogInformation($"{DateTime.Now}|\t Executing query to get DB type for datasource ID: {datasourceId}" );
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader.IsDBNull(0) ? null : reader.GetString(0);
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}|\t {ex.Message}");
                throw;
            }

            return null;
        }

        private async Task<IDataSource> GetDataSourceDb(int datasourceId)
        {
            if (datasourceId <= 0)
            {
                _logger.LogError($"{DateTime.Now}|\t Datasource ID must be greater than zero\", {nameof(datasourceId)}");
                throw new ArgumentException("Datasource ID must be greater than zero", nameof(datasourceId));
            }

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, name, provider, type, server, database, dbuser,dbpassword FROM DATASOURCE WHERE id=$datasourceId";
                    command.Parameters.AddWithValue("$datasourceId", datasourceId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new DataSource
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Provider = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Type = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Server = reader.IsDBNull(4) ? null : reader.GetString(4),
                                DataBase = reader.IsDBNull(5) ? null : reader.GetString(5),
                                DbUser = reader.IsDBNull(6) ? null : reader.GetString(6),
                                DbPassword = reader.IsDBNull(7) ? null : reader.GetString(7)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}|\t {ex.Message}");
                throw;
            }
            return null;
        }

        public async Task<BaseConBuilder> GetDbBuilder(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"{DateTime.Now}|\t Invalid datasource ID: {id}");
                throw new ArgumentException("Datasource ID must be greater than zero", nameof(id));
            }
            try
            {
                _logger.LogInformation($"{DateTime.Now}|\t Getting DB builder for datasource ID: {id}");
                string dbType = await GetTypeDb(id);
                IDataSource source = await GetDataSourceDb(id);
                switch (dbType)
                {
                    case "oracle":
                        _logger.LogInformation($"{DateTime.Now}|\t Creating OracleDBBuilder for datasource ID: {id}");
                        return new OracleDBBuilder(source, _configuration, _logger);
                    case "postgresql":
                        return new PostgreSqlDbBuilder(source, _configuration, _logger);
                    default:
                        return new BaseConBuilder(_configuration, _logger);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now}|\t {ex}, \nError occurred while getting DB builder for datasource ID: {id}");
                throw;
            }
        }
    }
}
