using DAPManSWebReports.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;

using Oracle.ManagedDataAccess.Client;

using System.Data;
using System.Data.Common;

namespace DAPManSWebReports.Infrastructure.DbBuilder
{
    public enum DatabaseType
    {
        oracle,
        postgresql,
        sqlite
    }
    public class DatabaseConnection : IDatabaseConnection
    {
        private static bool _isInitialized = false;
        private static readonly object LockObj = new();
        private readonly DbConnection _dbConn;
        private readonly DbCommand _dbComand;
        private readonly DbProviderFactory _dbFactory = (DbProviderFactory)null;
        private DbTransaction _dbTransaction;
        private DbDataAdapter _dbAdapter;
        private readonly DatabaseType _dbSourceType;
        public DatabaseType DbSourceType{get;} 

        private IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _provider;

        static DatabaseConnection()
        {
            InitializeProviderFactories();
        }

        private static void InitializeProviderFactories()
        {
            lock (LockObj)
            {
                if (_isInitialized) return;

                DbProviderFactories.RegisterFactory(
                    "Oracle.ManagedDataAccess.Client",
                    OracleClientFactory.Instance
                );
                DbProviderFactories.RegisterFactory(
                    "Npgsql",
                    Npgsql.NpgsqlFactory.Instance
                );
                DbProviderFactories.RegisterFactory(
                    "System.Data.SQLite",
                    System.Data.SQLite.SQLiteFactory.Instance
                );

                _isInitialized = true;
            }
        }

        public DatabaseConnection(string provider, string connectionString)
        {
            _provider                = provider;
            _dbFactory               = DbProviderFactories.GetFactory(GetProviderName(_provider));
            _dbConn                  = _dbFactory.CreateConnection();
            _dbAdapter               = _dbFactory.CreateDataAdapter();     
            _dbSourceType            = GetProviderType(_provider);                               
            _connectionString        = connectionString ?? throw new ArgumentException("Connection string cannot be null or empty");
            _dbConn.ConnectionString = _connectionString;
            _dbComand                = _dbConn.CreateCommand();
        }

        private string GetParamNameSymbol()
        {
            return _dbSourceType switch
            {
                DatabaseType.oracle => ":",
                DatabaseType.postgresql => "@",
                DatabaseType.sqlite => "@",
                _ => throw new NotSupportedException("Unsupported database type")
            };
        }
        public void AddParameter(string name, object value, DbType type)
        {
            string paramNameSymbol  = GetParamNameSymbol();
            name                    = name.Replace("@", paramNameSymbol);
            DbParameter parameter   = _dbFactory.CreateParameter();
            parameter.DbType        = type;
            parameter.Value         = value;
            parameter.ParameterName = name;
            _dbComand.Parameters.Add(parameter);
        }

        private string GetProviderName(string dbType)
        {
            return dbType.ToLower() switch
            {
                "oracle" => "Oracle.ManagedDataAccess.Client",
                "postgresql" => "Npgsql",
                "sqlite" => "System.Data.SQLite",
                _ => throw new NotSupportedException("Unsupported database type")
            };
        }

        private DatabaseType GetProviderType(string dbType)
        {
            return dbType.ToLower() switch
            {
                "oracle" => DatabaseType.oracle,
                "postgresql" => DatabaseType.postgresql,
                "sqlite" => DatabaseType.sqlite,
                _ => throw new NotSupportedException("Unsupported database type")
            };
        }
        public async Task<DataTable> ExecuteQuery(string query)
        {
            string paramNameSymbol = GetParamNameSymbol();
            query = query.Replace("@", paramNameSymbol);
            _dbComand.Connection = _dbConn;
            _dbComand.CommandText = query;
            if (_dbTransaction != null)
                _dbComand.Transaction = _dbTransaction;
            DataTable dt = new DataTable();
            DataSet dataSet = new DataSet();
            try
            {
                _dbAdapter.SelectCommand = _dbComand ;
                _dbAdapter.Fill(dataSet);
            }
            catch(OracleException ora)
            {
                Console.WriteLine(ora.Message);
            }
            catch(InvalidCastException ice)
            {
                Console.WriteLine(ice.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _dbComand.Parameters.Clear();
            return dataSet.Tables[dataSet.Tables.Count - 1];
        }
    }
}
