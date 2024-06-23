using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;

using Microsoft.Extensions.Configuration;

using System.Data.SQLite;

namespace DAPManSWebReports.Entities.Repositories.Implement
{
    public class FolderRepo : IFolderRepo<Folder>, IBaseRepo<Folder>
    {
        private readonly string _connectionString;
 
        public FolderRepo(IConfiguration configuration )
        {
            _connectionString = configuration.GetConnectionString("LocalConnection");
        }
        public Task<Folder> Create(Folder obj)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<Folder>> GetAll()
        {
            var items = new List<Folder>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, parentid, name, path, type, remoteuser, remotepassword, lastupdate, lastuser, system FROM FOLDER";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        items.Add(new Folder
                        {
                           Id             = reader.GetInt32(0),
                           ParentID       = reader.GetInt32(1),
                           Name           = reader.IsDBNull(2) ? null : reader.GetString(2),
                           Path           = reader.IsDBNull(3) ? null : reader.GetString(3),
                           Type           = reader.IsDBNull(4) ? null : reader.GetString(4),
                           RemoteUser     = reader.IsDBNull(5) ? null : reader.GetString(5),
                           RemotePassword = reader.IsDBNull(6) ? null : reader.GetString(6),
                           LastUpdate     = reader.GetDateTime(7),
                           LastUser       = reader.IsDBNull(8) ? null : reader.GetString(8),
                            System        = reader.GetBoolean(9)

                        });
                    }
                }
            }
            return items;
        }
        public async void InsertFavoriteFolder<T>(T obj)
        {
            throw new NotImplementedException();
        }
        public async Task<Folder> ReadById(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, parentid, name, path, type, remoteuser, remotepassword, lastupdate, lastuser, system FROM FOLDER  WHERE id = $id";
                command.Parameters.AddWithValue("$id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Folder
                        {
                            Id             = reader.GetInt32(0),
                            ParentID       = reader.GetInt32(1),
                            Name           = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Path           = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Type           = reader.IsDBNull(4) ? null : reader.GetString(4),
                            RemoteUser     = reader.IsDBNull(5) ? null : reader.GetString(5),
                            RemotePassword = reader.IsDBNull(6) ? null : reader.GetString(6),
                            LastUpdate     = reader.GetDateTime(7),
                            LastUser       = reader.IsDBNull(8) ? null : reader.GetString(8),
                            System         = reader.GetBoolean(9)
                        };
                    }
                }
            }
            return null;
        }
        public Task<IEnumerable<Folder>> ReadListByMatch(string entity, string match, string orderField)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Folder> ReadListByParentID(int parentID)
        {
            var items = new List<Folder>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT id, parentid, name, path, type, remoteuser, remotepassword, lastupdate, lastuser, system FROM FOLDER WHERE parentID = $id";
                command.Parameters.AddWithValue("$id", parentID);
                using (var reader = command.ExecuteReader())
                {
                    while ( reader.Read())
                    {
                        items.Add(new Folder()
                        {
                            Id = reader.GetInt32(0),
                            ParentID = reader.GetInt32(1),
                            Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Path = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Type = reader.IsDBNull(4) ? null : reader.GetString(4),
                            RemoteUser = reader.IsDBNull(5) ? null : reader.GetString(5),
                            RemotePassword = reader.IsDBNull(6) ? null : reader.GetString(6),
                            LastUpdate = reader.GetDateTime(7),
                            LastUser = reader.IsDBNull(8) ? null : reader.GetString(8),
                            System = reader.GetBoolean(9)
                        });
                    }
                }
            }
            return items;
        }
        public async Task<IEnumerable<Folder>> ReadListParentEntities()
        {
            var items = new List<Folder>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                //command.CommandText = "SELECT id, name from FOLDER WHERE ID in(select PARENTID from folder)";
                command.CommandText = "SELECT id, parentid, name, path, type, remoteuser, remotepassword, lastupdate, lastuser, system from FOLDER WHERE PARENTID in(-20) AND ID IN(83,74,52)";
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        items.Add(new Folder
                        {
                            Id = reader.GetInt32(0),
                            ParentID = reader.GetInt32(1),
                            Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Path = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Type = reader.IsDBNull(4) ? null : reader.GetString(4),
                            RemoteUser = reader.IsDBNull(5) ? null : reader.GetString(5),
                            RemotePassword = reader.IsDBNull(6) ? null : reader.GetString(6),
                            LastUpdate = reader.GetDateTime(7),
                            LastUser = reader.IsDBNull(8) ? null : reader.GetString(8),
                            System = reader.GetBoolean(9)
                        });
                    }
                }
            }
            return items;
        }
        public async void Update(Folder obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateDataAsync(Folder obj)
        {
            throw new NotImplementedException();
        }
    }
}
