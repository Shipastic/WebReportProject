using DAPManSWebReports.Infrastructure.Interfaces;

using Microsoft.Extensions.Configuration;

using Npgsql;

using Oracle.ManagedDataAccess.Client;

using System.Data;
using System.Data.Common;
using System.Linq;
using System.Windows.Input;

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
        private static bool                 _isInitialized = false;
        private static readonly object      LockObj = new();
        private readonly DbConnection       _dbConn;
        private readonly DbCommand          _dbCommand;
        private readonly DbProviderFactory  _dbFactory = (DbProviderFactory)null;
        private List<OracleParameter>       _parametersOracle;
        private List<NpgsqlParameter>       _parametersPostgresql;
        private DbTransaction               _dbTransaction;
        private DbDataAdapter               _dbAdapter;
        private readonly DatabaseType       _dbSourceType;
        public DatabaseType                 DbSourceType{get;} 
        private IConfiguration              _configuration;
        private readonly string             _connectionString;
        private readonly string             _provider;

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
            _dbCommand               = _dbConn.CreateCommand();
            _parametersOracle        = new List<OracleParameter>();
            _parametersPostgresql    = new List<NpgsqlParameter>();
        }

        /// <summary>
        /// Возвращает символ для параметра, в зависимости от бд
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private string GetParamNameSymbol()
        {
            return _dbSourceType switch
            {
                DatabaseType.oracle     => ":",
                DatabaseType.postgresql => "@",
                DatabaseType.sqlite     => "@",
                _                       => throw new NotSupportedException("Unsupported database type")
            };
        }
        
        /// <summary>
        /// Добавляет параметр в команду бд
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public void AddParameter(string name, object value, DbType type)
        {
            string paramNameSymbol  = GetParamNameSymbol();
            name                    = name.Replace("@", paramNameSymbol);
            DbParameter parameter   = _dbFactory.CreateParameter();
            parameter.DbType        = type;
            parameter.Value         = value;
            parameter.ParameterName = name;
            if (_dbSourceType == DatabaseType.oracle)
            {
                if (!_parametersOracle.Any(n => n.ParameterName == parameter.ParameterName)) 
                {
                    _parametersOracle.Add(new OracleParameter(parameter.ParameterName, parameter.Value));
                }
            }
            else if(_dbSourceType == DatabaseType.postgresql)
            {
                _parametersPostgresql.Add(new NpgsqlParameter(parameter.ParameterName, parameter.Value));
            }
        }

        /// <summary>
        /// Возвращает название провайдера бд
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
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

        /// <summary>
        /// Возвращает тип бд
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
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
        /// <summary>
        /// Выполняет запрос и возвращает результат
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteQuery(string query)
        {
            _dbCommand.Connection = _dbConn;
            _dbCommand.CommandText = query.TrimEnd();
            DataTable dataTable = new DataTable();
            try
            {
                Console.WriteLine("Final Query: " + _dbCommand.CommandText);
                if (_parametersPostgresql.Count != 0)
                {
                    foreach (var param in _parametersPostgresql)
                    {
                        Console.WriteLine($"Parameter Name: {param.ParameterName}, Value: {param.Value}, Type: {param.DbType}, Oracle Type: {param.NpgsqlDbType}");
                        _dbCommand.Parameters.Add(param);
                    }
                }
                else
                    if (_parametersOracle.Count != 0)
                {
                    foreach (var param in _parametersOracle)
                    {
                        Console.WriteLine($"Parameter Name: {param.ParameterName}, Value: {param.Value}, Type: {param.DbType}, Oracle Type: {param.OracleDbType}");
                        _dbCommand.Parameters.Add(param);
                    }
                    using var adapter = new OracleDataAdapter((OracleCommand)_dbCommand) { SuppressGetDecimalInvalidCastException = true };

                    adapter.Fill(dataTable);
                }             
            }
            catch (FormatException ex)
            {
                // Логирование ошибки формата
                Console.WriteLine("FormatException: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
            }
            catch (OracleException ora)
            {
                Console.WriteLine(ora.Message);
                return null;
            }
            catch(InvalidCastException ice)
            {
                Console.WriteLine(ice.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                _dbCommand.Parameters.Clear();
            }
            
            return dataTable;
        }
    }
}
