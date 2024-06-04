using DAPManSWebReports.Infrastructure.Interfaces;
using DAPManSWebReports.Infrastructure.Models;

using Microsoft.Extensions.Configuration;

using System.Data.SQLite;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public class BaseConBuilder : IBaseConBuilder
    {
        private readonly string _connectionString;

        private IConfiguration _configuration;

        public BaseConBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("LocalConnection");
        }

        private async Task<string> GetTypeDb(int datasourceId)
        {
            string type = "";
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT lower(type) FROM DATASOURCE WHERE id=$datasourceId";
                command.Parameters.AddWithValue("$datasourceId", datasourceId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        type = reader.IsDBNull(0) ? null : reader.GetString(0);
                        return type;
                    }
                }
            }
            return null;
        }

        private async Task<IDataSource> GetDataSourceDb(int datasourceId)
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
                            Id         = reader.GetInt32(0),
                            Name       = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Provider   = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Type       = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Server     = reader.IsDBNull(4) ? null : reader.GetString(4),
                            DataBase   = reader.IsDBNull(5) ? null : reader.GetString(5),
                            DbUser     = reader.IsDBNull(6) ? null : reader.GetString(6),
                            DbPassword = reader.IsDBNull(7) ? null : reader.GetString(7)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<BaseConBuilder> GetDbBuilder(int id)
        {
            string dbType = await GetTypeDb(id);
            IDataSource source = await GetDataSourceDb(id);
            switch (dbType)
            {
                case "oracle":
                    return new OracleDBBuilder(source, _configuration);
                case "postgresql":
                    return new PostgreSqlDbBuilder(source, _configuration);
                default:
                    return new BaseConBuilder(_configuration);
            }
        }
    }
}
