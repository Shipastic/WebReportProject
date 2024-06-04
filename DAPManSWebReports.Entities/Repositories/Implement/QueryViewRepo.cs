using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

using Oracle.ManagedDataAccess.Client;

using System.Data;

namespace DAPManSWebReports.Entities.Repositories.Implement
{
    public class QueryViewRepo: IQueryRepo<QueryView>
    {
        private readonly IBaseConBuilder _baseConBuilder;

        private readonly IBaseRepo<Models.DataView> _dataViewRepo;

        public QueryViewRepo(IBaseConBuilder baseConBuilder, IBaseRepo<Models.DataView> dataViewRepo)
        {
            _baseConBuilder = baseConBuilder ?? throw new ArgumentNullException(nameof(baseConBuilder));
            _dataViewRepo   = dataViewRepo   ?? throw new ArgumentNullException(nameof(dataViewRepo));
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
                using (OracleConnection con = new OracleConnection(dbOrclstring))
                {
                    try
                    {
                        await con.OpenAsync();
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.CommandText = $"{dvRes.Query}  OFFSET {offset} ROWS FETCH NEXT {limit} ROWS ONLY";
                            using var adapter = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
                            try
                            {
                                adapter.Fill(dt);
                            }
                            catch (InvalidCastException ex)
                            {
                                Console.WriteLine($"An error occurred during mapping: {ex.Message}");
                                foreach (var er in dt.GetErrors())
                                {
                                    Console.WriteLine($"{er.GetType()}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}\t{ex.InnerException}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
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

            var dvRes = await _dataViewRepo.ReadById(dataviewId);
            if (dvRes == null)
            {
                throw new KeyNotFoundException($"DataView with ID {dataviewId} not found.");
            }
            var dtSource = await _baseConBuilder.GetDbBuilder(dvRes.DataSourceId);
            if (dtSource is OracleDBBuilder builder)
            {
                string dbOrclstring = builder.GetConnectionStringDb();
                using (OracleConnection con = new OracleConnection(dbOrclstring))
                {
                    try
                    {
                        await con.OpenAsync();
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            string whereClause = "";
                            if (queryparams.TryGetValue("startDate", out var startDate) 
                                && queryparams.TryGetValue("endDate", out var endDate)
                                && queryparams.TryGetValue("offset", out var offset)
                                && queryparams.TryGetValue("limit", out var limit))
                            {
                                
                                whereClause = $" WHERE {dvRes.StartDateField} >= TO_DATE(:startDate, 'YYYY-MM-DD') AND {dvRes.StopDateField} <= TO_DATE(:endDate, 'YYYY-MM-DD') OFFSET :offset ROWS FETCH NEXT :limit ROWS ONLY";
                                cmd.Parameters.Add(new OracleParameter("startDate", (string)startDate));
                                cmd.Parameters.Add(new OracleParameter("endDate", (string)endDate));
                                cmd.Parameters.Add(new OracleParameter("offset", (int)offset));
                                cmd.Parameters.Add(new OracleParameter("limit", (int)limit));
                            }
                            
                            cmd.CommandText = $"{dvRes.Query} {whereClause}";
                            using var adapter = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
                            try
                            {
                                adapter.Fill(dt);
                            }
                            catch (InvalidCastException ex)
                            {
                                Console.WriteLine($"An error occurred during mapping: {ex.Message}");
                                foreach (var er in dt.GetErrors())
                                {
                                    Console.WriteLine($"{er.GetType()}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}\t{ex.InnerException}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw new Exception($"An error occurred while reading data: {ex.Message}", ex);
                    }
                }
            }
            return new QueryView(dvRes)
            {
                TableResultQuery = dt,
                Title = dvRes.Name
            };
        }

        public async Task<int> GetCountById(int dataviewId, Dictionary<string, object> queryparams)
        {
            var dvRes = await _dataViewRepo.ReadById(dataviewId);
            int rowCount = 0;
            if (dvRes == null)
            {
                throw new KeyNotFoundException($"DataView with ID {dataviewId} not found.");
            }
            var dtSource = await _baseConBuilder.GetDbBuilder(dvRes.DataSourceId);
            if (dtSource is OracleDBBuilder builder)
            {
                string dbOrclstring = builder.GetConnectionStringDb();
                using (OracleConnection con = new OracleConnection(dbOrclstring))
                {
                    try
                    {
                        await con.OpenAsync();
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            string whereClause = "";
                            if (queryparams.TryGetValue("startDate", out var startDate)
                                && queryparams.TryGetValue("endDate", out var endDate))
                            {

                                whereClause = $" WHERE {dvRes.StartDateField} >= TO_DATE(:startDate, 'YYYY-MM-DD') AND {dvRes.StopDateField} <= TO_DATE(:endDate, 'YYYY-MM-DD')";
                                cmd.Parameters.Add(new OracleParameter("startDate", (string)startDate));
                                cmd.Parameters.Add(new OracleParameter("endDate", (string)endDate));                            
                            }

                            cmd.CommandText = $"SELECT COUNT(*) FROM ({dvRes.Query} {whereClause})";
                            
                            try
                            {
                                rowCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                            }                            
                            catch (Exception ex)
                            {
                                Console.WriteLine($"{ex.Message}\t{ex.InnerException}");
                                return 0;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                        throw new Exception($"An error occurred while reading data: {ex.Message}", ex);
                    }
                }
            }
            return rowCount;
        }
    }
}
