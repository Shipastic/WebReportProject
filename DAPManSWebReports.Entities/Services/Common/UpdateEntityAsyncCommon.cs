using Microsoft.Extensions.Configuration;

using System.Data.SQLite;
using System.Reflection;


namespace DAPManSWebReports.Entities.Services.Common
{
    public class UpdateEntityAsyncCommon(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("LocalConnection");

        public async Task<bool> UpdateEntityAsync<T>(T existingEntity, T newEntity, string tableName, string idColumnName) where T : class
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var updates = new List<string>();
                        var parameters = new List<SQLiteParameter>();

                        PropertyInfo[] properties = typeof(T).GetProperties();

                        foreach (var property in properties)
                        {
                            var existingValue = property.GetValue(existingEntity);
                            var newValue = property.GetValue(newEntity);

                            if (!Equals(existingValue, newValue))
                            {
                                updates.Add($"{property.Name} = @{property.Name}");
                                parameters.Add(new SQLiteParameter($"@{property.Name}", newValue ?? DBNull.Value));
                            }
                        }

                        if (updates.Count == 0)
                        {
                            return true; // Нет изменений
                        }

                        var idProperty = typeof(T).GetProperty(idColumnName);
                        if (idProperty == null)
                        {
                            throw new ArgumentException($"Property '{idColumnName}' not found on type '{typeof(T).Name}'");
                        }

                        var idValue = idProperty.GetValue(newEntity);

                        var updateQuery = $"UPDATE {tableName} SET {string.Join(", ", updates)} WHERE {idColumnName} = @{idColumnName}";
                        parameters.Add(new SQLiteParameter($"@{idColumnName}", idValue));

                        var updateCommand = new SQLiteCommand(updateQuery, connection, transaction);
                        updateCommand.Parameters.AddRange(parameters.ToArray());

                        var rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
