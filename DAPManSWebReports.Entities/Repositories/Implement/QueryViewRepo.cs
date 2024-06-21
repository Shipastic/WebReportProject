using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

using Oracle.ManagedDataAccess.Client;
using System.Data;
using LoggingLibrary.Service;
using Microsoft.Extensions.Logging;

namespace DAPManSWebReports.Entities.Repositories.Implement
{
    public class QueryViewRepo: IQueryRepo<QueryView>
    {
        private readonly IBaseConBuilder _baseConBuilder;

        private readonly IBaseRepo<Models.DataView> _dataViewRepo;
        private readonly ILogger<QueryViewRepo> _logger;

        public QueryViewRepo(IBaseConBuilder baseConBuilder, IBaseRepo<Models.DataView> dataViewRepo, ILogger<QueryViewRepo> logger)
        {
            _baseConBuilder = baseConBuilder ?? throw new ArgumentNullException(nameof(baseConBuilder));
            _dataViewRepo   = dataViewRepo   ?? throw new ArgumentNullException(nameof(dataViewRepo));
            _logger         = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<QueryView> ReadById(int dataviewId, int limit, int offset)
        {
            DataTable dt = new DataTable();

            var dvRes = await _dataViewRepo.ReadById(dataviewId);
            if (dvRes == null)
            {
                throw new KeyNotFoundException($"DataView with ID {dataviewId} not found.");
            }
            var dtSource = await _baseConBuilder.GetDbBuilder(dvRes.DataSourceId);
            if (dtSource is OracleDBBuilder builder)
            {
                string dbOrclstring = builder.GetConnectionStringDb();

                _logger.LogInformation($" Create connection string  to db: {dbOrclstring}");

                using (OracleConnection con = new OracleConnection(dbOrclstring))
                {
                    try
                    {
                        await con.OpenAsync();

                        _logger.LogInformation($" Open connection to db");

                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandText = $"{dvRes.Query}";

                            _logger.LogInformation($" Formed Query:{cmd.CommandText}");

                            using var adapter = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
                            try
                            {
                                adapter.Fill(dt);
                                int rowCount = dt.Rows.Count;
                            }
                            catch (InvalidCastException ex)
                            {
                                Console.WriteLine($"An error occurred during mapping: {ex.Message}");

                                _logger.LogError($"An error occurred during mapping: {ex.Message}");

                                foreach (var er in dt.GetErrors())
                                {
                                    Console.WriteLine($"{er.GetType()}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}\t{ex.InnerException}");
                                _logger.LogError($"{ex.Message}\t{ex.InnerException}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");

                        _logger.LogError($"An error occurred: {ex.Message}");

                        throw new Exception($"An error occurred while reading data: {ex.Message}", ex);
                    }
                }
            }
            return new QueryView(dvRes)
            {
                TableResultQuery = dt ,
                Title            = dvRes.Name
            };
        }
        public IEnumerable<QueryView> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<QueryView> ReadById(int dataviewId, Dictionary<string, object> queryparams)
        {
            DataTable dt = new DataTable();
            int totalCount = 0;
            string query = "";
            var dvRes = await _dataViewRepo.ReadById(dataviewId);
            if (dvRes == null)
            {
                _logger.LogError($"{DateTime.Now}|\t DataView with ID {dataviewId} not found.");

                throw new KeyNotFoundException($"DataView with ID {dataviewId} not found.");               
            }
            var dtSource = await _baseConBuilder.GetDbBuilder(dvRes.DataSourceId);

            if (dtSource is OracleDBBuilder builder)
            {
                string dbOrclstring = builder.GetConnectionStringDb();

                _logger.LogInformation($"{DateTime.Now}|\t  Create connection string  to db: {dbOrclstring}");

                using (OracleConnection con = new OracleConnection(dbOrclstring))
                {
                    try
                    {
                        await con.OpenAsync();
                        
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            QueryBuilder queryBuilder = new QueryBuilder(dvRes.Query);
                            if (!string.IsNullOrEmpty(dvRes.StartDateField) && !string.IsNullOrEmpty(dvRes.StopDateField)
                                && queryparams.TryGetValue("startDate", out var startDate)
                                && queryparams.TryGetValue("endDate", out var endDate) 
                                && !string.IsNullOrEmpty(Convert.ToString(startDate)) 
                                && !string.IsNullOrEmpty(Convert.ToString(endDate)))
                            {
                                queryBuilder.AddDateFilter(dvRes.StartDateField, dvRes.StopDateField, Convert.ToDateTime(startDate), Convert.ToDateTime(endDate));
                            }

                            query = queryBuilder.BuildQuery();

                            cmd.CommandText = query;
                            _logger.LogInformation($"{DateTime.Now}|\t  Create Query to db: {query}");

                            var parameters = queryBuilder.GetParameters();
                            foreach (var parameter in parameters)
                            {
                                cmd.Parameters.Add(parameter);
                            }

                            using var adapter = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
                            try
                            {
                                adapter.Fill(dt);
                                totalCount = dt.Rows.Count;
                            }
                            catch (InvalidCastException ex)
                            {
                               _logger.LogError($"{DateTime.Now}|\t An error occurred during mapping: {ex.Message}");

                                foreach (var er in dt.GetErrors())
                                {
                                    _logger.LogError($"{er.GetType()}");
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError($"{DateTime.Now}|\t {ex.Message}\t{ex.InnerException}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{DateTime.Now}|\t An error occurred: {ex.Message}");
                        throw new Exception($"An error occurred while reading data: {ex.Message}", ex);
                    }
                }
            }
            return new QueryView(dvRes)
            {
                TableResultQuery = dt,
                Title = dvRes.Name,
                ResultQuery = query,
                TotalCount = totalCount
            };
        }
    }
}
