
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;
using DAPManSWebReports.Entities.Services.Common;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Oracle.ManagedDataAccess.Client;

using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SqlTypes;

namespace DAPManSWebReports.Entities.Repositories.Implement
{
    public class DataViewRepo : IBaseRepo<DataView>, IDataViewRepo<DataView>
    {
        private readonly string _connectionString;

        private readonly ILogger<DataViewRepo> _logger;

        private readonly IConfiguration _configuration;

        public DataViewRepo(IConfiguration configuration, ILogger<DataViewRepo> logger)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("LocalConnection");
            _logger = logger;
        }
        public async Task<DataView> Create(DataView dv)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO DATAVIEW (DataSourceID, FolderID, Name, Query, Id) VALUES ($datasourceId, $folderId, $name, $query)";

                command.Parameters.AddWithValue("$datasourceId", dv.DataSourceId);
                command.Parameters.AddWithValue("$folderId", dv.FolderId);
                command.Parameters.AddWithValue("$name", dv.Name);
                command.Parameters.AddWithValue("$query", dv.Query);

                await command.ExecuteNonQueryAsync();
                return dv;
            }
        }
        public async void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM DATAVIEW WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task<IEnumerable<DataView>> GetAll()
        {
            _logger.LogInformation($"{DateTime.Now}|\t Get All DataView Calls");
            var items = new List<DataView>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT DataSourceID, FolderID, Id, Name, Query, StartDateField, StopDateField, Dataviewnote FROM DATAVIEW";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        items.Add(new DataView
                        {
                            DataSourceId   = reader.GetInt32(0),
                            FolderId       = reader.GetInt32(1),
                            id             = reader.GetInt32(2),
                            Name           = reader.IsDBNull(3) || string.IsNullOrWhiteSpace(reader.GetString(3)) ? null : reader.GetString(3),
                            Query          = reader.IsDBNull(4) || string.IsNullOrWhiteSpace(reader.GetString(4)) ? null : reader.GetString(4),
                            StartDateField = reader.IsDBNull(5) || string.IsNullOrWhiteSpace(reader.GetString(5)) ? null : reader.GetString(5),
                            StopDateField  = reader.IsDBNull(6) || string.IsNullOrWhiteSpace(reader.GetString(6)) ? null : reader.GetString(6),
                            DataViewNote   = reader.IsDBNull(7) || string.IsNullOrWhiteSpace(reader.GetString(7)) ? null : reader.GetString(7)
                        });
                    }
                }
            }
            return items;
        }
        public async Task<DataView> ReadById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT DataSourceID, FolderID, Id, Name, Query, StartDateField, StopDateField, Dataviewnote FROM DATAVIEW WHERE Id = $id";
                command.Parameters.AddWithValue("$id", id);

                _logger.LogInformation($"{DateTime.Now}|\t Get DataView by id:{id}");
                _logger.LogInformation($"{DateTime.Now}|\t With SQL QUERY: {command.CommandText}");
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new DataView
                        {
                            DataSourceId   = reader.GetInt32(0),
                            FolderId       = reader.GetInt32(1),
                            id             = reader.GetInt32(2),
                            Name           = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Query          = reader.IsDBNull(4) ? null : reader.GetString(4),
                            StartDateField = reader.IsDBNull(5) ? null : reader.GetString(5),
                            StopDateField  = reader.IsDBNull(6) ? null : reader.GetString(6),
                            DataViewNote   = reader.IsDBNull(7) ? null : reader.GetString(7)
                        };
                    }
                }
            }
            return null;
        }
        public IEnumerable<DataView> ReadListByParentID(int parentID)
        {
            var items = new List<DataView>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT DataSourceID, FolderID, Id, Name, Query, StartDateField, StopDateField, Dataviewnote FROM DATAVIEW WHERE FOLDERID  = $id";
                command.Parameters.AddWithValue("$id", parentID);
                using (var reader =  command.ExecuteReader())
                {
                    while ( reader.Read())
                    {
                        items.Add(new DataView()
                        {
                            DataSourceId   = reader.GetInt32(0),
                            FolderId       = reader.GetInt32(1),
                            id             = reader.GetInt32(2),
                            Name           = reader.IsDBNull(3) || string.IsNullOrWhiteSpace(reader.GetString(3)) ? null : reader.GetString(3),
                            Query          = reader.IsDBNull(4) || string.IsNullOrWhiteSpace(reader.GetString(4)) ? null : reader.GetString(4),
                            StartDateField = reader.IsDBNull(5) || string.IsNullOrWhiteSpace(reader.GetString(5)) ? null : reader.GetString(5),
                            StopDateField  = reader.IsDBNull(6) || string.IsNullOrWhiteSpace(reader.GetString(6)) ? null : reader.GetString(6),
                            DataViewNote   = reader.IsDBNull(7) || string.IsNullOrWhiteSpace(reader.GetString(7)) ? null : reader.GetString(7)
                        });
                    }
                }
            }
            return items;
        }

        public async Task<IEnumerable<DataView>> ReadListFromFolderList(List<int> folderListId)
        {
            var items = new List<DataView>();

            string resultFolderIds = string.Join(",", folderListId);

            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT id, name, Query, datasourceid, Dataviewnote, folderid FROM DATAVIEW WHERE FOLDERID IN ({resultFolderIds})";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        items.Add(new DataView
                        {
                            id           = reader.GetInt32(0),
                            DataSourceId = reader.GetInt32(3),
                            FolderId     = reader.GetInt32(5),

                            Name         = reader.IsDBNull(1) || string.IsNullOrWhiteSpace(reader.GetString(1)) ? null : reader.GetString(1),
                            Query        = reader.IsDBNull(2) || string.IsNullOrWhiteSpace(reader.GetString(2)) ? null : reader.GetString(2),                       
                            DataViewNote = reader.IsDBNull(4) || string.IsNullOrWhiteSpace(reader.GetString(4)) ? null : reader.GetString(4),
                            
                        });
                    }
                }
            }
            return items;
        }

        public async Task<IEnumerable<DataView>> ReadListParentEntities()
        {
            var items = new List<DataView>();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT DataSourceID, FolderID, Id, Name, Query, StartDateField, StopDateField, Dataviewnote FROM DATAVIEW WHERE FOLDERID = -20";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        items.Add(new DataView
                        {
                            DataSourceId   = reader.GetInt32(0),
                            FolderId       = reader.GetInt32(1),
                            id             = reader.GetInt32(2),
                            Name           = reader.IsDBNull(3) || string.IsNullOrWhiteSpace(reader.GetString(3)) ? null : reader.GetString(3),
                            Query          = reader.IsDBNull(4) || string.IsNullOrWhiteSpace(reader.GetString(4)) ? null : reader.GetString(4),
                            StartDateField = reader.IsDBNull(5) || string.IsNullOrWhiteSpace(reader.GetString(5)) ? null : reader.GetString(5),
                            StopDateField  = reader.IsDBNull(6) || string.IsNullOrWhiteSpace(reader.GetString(6)) ? null : reader.GetString(6),
                            DataViewNote   = reader.IsDBNull(7) || string.IsNullOrWhiteSpace(reader.GetString(7)) ? null : reader.GetString(7)
                        });
                    }
                }
            }
            return items;
        }
        public async void Update(DataView dv)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "UPDATE DATAVIEW SET DataSourceID = $datasourceId, FolderID = $folderId, Name = $name, Query = $query WHERE Id = $id";
                command.Parameters.AddWithValue("$datasourceId", dv.DataSourceId);
                command.Parameters.AddWithValue("$folderId", dv.FolderId);
                command.Parameters.AddWithValue("$name", dv.Name);
                command.Parameters.AddWithValue("$query", dv.Query);
                command.Parameters.AddWithValue("$id", dv.id);

                await command.ExecuteNonQueryAsync();
            }
        }

        public Task<bool> UpdateEntityAsync(DataView existingEntity, DataView newEntity, string tableName, string idColumnName)
        {
            UpdateEntityAsyncCommon updateEntityAsyncCommon = new UpdateEntityAsyncCommon(_configuration);

            return updateEntityAsyncCommon.UpdateEntityAsync(existingEntity, newEntity, tableName, idColumnName);
        }
    }
}
