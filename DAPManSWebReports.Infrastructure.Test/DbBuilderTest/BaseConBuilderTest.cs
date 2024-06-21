
using Microsoft.Extensions.Configuration;
using DAPManSWebReports.Infrastructure.DbBuilder;
using System.Data.SQLite;
using Moq;
using Microsoft.Extensions.Logging;
using System.Configuration;

namespace DAPManSWebReports.Infrastructure.Test.DbBuilderTest
{
    public class BaseConBuilderTest
    {
        private const string ConnectionString = "Data Source=local.db;Version=3;New=True;";
        private readonly Mock<ILogger<BaseConBuilder>> _mockLogger;
        private readonly BaseConBuilder _baseConBuilder;
        private readonly IConfiguration _configuration;
        private readonly ILogger<BaseConBuilder> _logger;

        public BaseConBuilderTest()
        {
            _mockLogger = new Mock<ILogger<BaseConBuilder>>();
            _logger = _mockLogger.Object;

            _configuration = BuildConfiguration();
            InitializeDatabase();

            _baseConBuilder = new BaseConBuilder(_configuration, _logger);
        }

        private IConfiguration BuildConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
        {
            {"ConnectionStrings:LocalConnection", ConnectionString}
        };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }
        private void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.OpenAsync();

                string createTableQuery = @"
                DROP TABLE IF EXISTS DATASOURCE;
                CREATE TABLE DATASOURCE (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT,
                    provider TEXT,
                    type TEXT,
                    server TEXT,
                    database TEXT,
                    dbuser TEXT,
                    dbpassword TEXT
                );
                INSERT INTO DATASOURCE (id, name, type, provider,server, database) VALUES (1, '1', 'oracle', 'oracle', 'mes-db-pre', 'tpc');";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQueryAsync();
                }
            }
        }

        [Fact]
        public void Constructor_ValidConfiguration_SetsConnectionString()
        {
            // Act
            var builder = new BaseConBuilder(_configuration, _logger);

            // Assert
            Assert.NotNull(builder);
        }

        [Fact]
        public async Task GetTypeDb_ValidDatasourceId_ReturnsCorrectType()
        {
            // Act
            var builder = new BaseConBuilder(_configuration, _logger);

            var dbBuilder = await builder.GetDbBuilder(1);

            // Assert
            Assert.IsType<OracleDBBuilder>(dbBuilder);
        }

        [Fact]
        public async Task GetDbBuilder_ValidId_ReturnsCorrectBuilder()
        {
            // Arrange - подготовка
            var builder = new BaseConBuilder(_configuration, _logger);

            // Act
            var dbBuilder = await builder.GetDbBuilder(1);

            // Assert
            Assert.IsType<OracleDBBuilder>(dbBuilder);
        }
    }
}
