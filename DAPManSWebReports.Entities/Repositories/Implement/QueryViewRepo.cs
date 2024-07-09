using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

using Oracle.ManagedDataAccess.Client;
using System.Data;
using LoggingLibrary.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using DAPManSWebReports.Entities.Services.QueryBuilder;

namespace DAPManSWebReports.Entities.Repositories.Implement
{
    public class QueryViewRepo: IQueryRepo<QueryView>
    {
        private readonly IBaseConBuilder _baseConBuilder;
        private readonly IConfiguration _configuration;
        private readonly IBaseRepo<Models.DataView> _dataViewRepo;
        private readonly ILogger<QueryViewRepo> _logger;
        public QueryViewRepo(IBaseConBuilder baseConBuilder, IBaseRepo<Models.DataView> dataViewRepo, ILogger<QueryViewRepo> logger, IConfiguration configuration)
        {
            _baseConBuilder = baseConBuilder ?? throw new ArgumentNullException(nameof(baseConBuilder));
            _dataViewRepo   = dataViewRepo   ?? throw new ArgumentNullException(nameof(dataViewRepo));
            _logger         = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration  = configuration;
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
            var queryContext = new QueryBuilderContext();
            var userInput    = queryparams["buildQuery"];
            DataTable dt     = new DataTable();
            int totalCount   = 0;
            string query     = "";
            string connectionDbString = "";
            var dvRes        = await _dataViewRepo.ReadById(dataviewId);
            dvRes.SetQueryParameters(queryparams);
            if (dvRes == null)
            {
                _logger.LogError($"{DateTime.Now}|\t DataView with ID {dataviewId} not found.");
                throw new KeyNotFoundException($"DataView with ID {dataviewId} not found.");               
            }
            var dtSource  = await _baseConBuilder.GetDbBuilder(dvRes.DataSourceId);
            string dbType = await dtSource.GetTypeDb(dvRes.DataSourceId); 
            
            if (dtSource is OracleDBBuilder builder)
            {
                connectionDbString = builder.GetConnectionStringDb();

                _logger.LogInformation($"{DateTime.Now}|\t  Create connection string  to db: {connectionDbString}");
            }
            DateTime StartDate = Convert.ToDateTime(dvRes.parameters.Where(p => p.Name.Equals("startDate"))
                                                                    .Select(v => v.Value)
                                                                    .SingleOrDefault());
            DateTime StopDate  = Convert.ToDateTime(dvRes.parameters.Where(p => p.Name
                                                                    .Equals("stopDate"))
                                                                    .Select(v => v.Value)
                                                                    .SingleOrDefault());
            if (userInput.Equals("NASOUP")) 
            {
                queryContext.SetQueryBuilderStrategy(new QueryBuilder(dvRes, connectionDbString)
                                                                            .AddDateFilter(dvRes.StartDateField,dvRes.StopDateField,StartDate,StopDate));
            }
            else
            if (userInput.Equals("Q3INTEL"))
            {
                queryContext.SetQueryBuilderStrategy(new QueryBuilderIntel(dvRes, dbType, connectionDbString, StartDate, StopDate));
            }
            
            query = queryContext.BuildQuery();

            dt = await queryContext.ExecuteQuery(query);
         
            _logger.LogInformation($"{DateTime.Now}|\t  Create Query to db: {query}");
            if (dt != null)
            {
                totalCount = dt.Rows.Count;
            }
            else
            {
                totalCount = 0;
            }

            return new QueryView(dvRes)
            {
                TableResultQuery = dt,
                Title = dvRes.Name,
                ResultQuery = query,
                TotalCount = totalCount
            };
        }

        /// <summary>
        /// Метод возвращает кортеж из названий столбцов и значений
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public (List<object> objectList1, List<object> objectList2) DataTableToDataviewData(DataTable dt)
        {
            List<object> objectList1 = new List<object>();
            List<object> objectList2 = new List<object>();
            //
            foreach (DataColumn column in (InternalDataCollectionBase)dt.Columns)
            {
                Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
                int ordinal;

                Dictionary<string, string> dictionary2 = dictionary1;
                ordinal = column.Ordinal;
                string key = "data" + ordinal.ToString();
                string columnName = column.ColumnName;
                dictionary2.Add(key, columnName);
                objectList2.Add((object)dictionary1);

            }
            //
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (DataColumn column in (InternalDataCollectionBase)dt.Columns)
                {
                    dictionary.Add("data" + column.Ordinal.ToString(), row[column].ToString());
                }
                objectList1.Add((object)dictionary);
            }

            return (objectList1, objectList2);
        }
    }
}
