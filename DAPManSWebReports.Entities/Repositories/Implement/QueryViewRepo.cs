using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Windows.Input;

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
                            cmd.CommandText = $"{dvRes.Query}";
                            using var adapter = new OracleDataAdapter(cmd) { SuppressGetDecimalInvalidCastException = true };
                            try
                            {
                                adapter.Fill(dt);
                                int rowCount = dt.Rows.Count;
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
            int totalCount = 0;
            string query = "";
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
                Title = dvRes.Name,
                ResultQuery = query,
                TotalCount = totalCount
            };
        }

        //public async Task<int> GetCountById(int dataviewId, Dictionary<string, object> queryparams, string queryString, int dataSourceId)
        //{
        //    int rowCount = 0;
        //    var dtSource = await _baseConBuilder.GetDbBuilder(dataSourceId);
        //    if (dtSource is OracleDBBuilder builder)
        //    {
        //        string dbOrclstring = builder.GetConnectionStringDb();
        //        using (OracleConnection con = new OracleConnection(dbOrclstring))
        //        {
        //            try
        //            {
        //                await con.OpenAsync();
        //                using (OracleCommand cmd = con.CreateCommand())
        //                {
        //                    QueryBuilder queryBuilder = new QueryBuilder(queryString);
        //                    cmd.CommandText = $"SELECT COUNT(*) FROM ({queryString})";
        //                    cmd.Parameters. Add(new OracleParameter("startDate", queryparams["startDate"].ToString()));
        //                    cmd.Parameters.Add(new OracleParameter("endDate", queryparams["endDate"].ToString()));
        //                    try
        //                    {
        //                        rowCount = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        //                    }                            
        //                    catch (Exception ex)
        //                    {
        //                        Console.WriteLine($"{ex.Message}\t{ex.InnerException}");
        //                        return 0;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"An error occurred: {ex.Message}");
        //                throw new Exception($"An error occurred while reading data: {ex.Message}", ex);
        //            }
        //        }
        //    }
        //    return rowCount;
        //}
    }
}
