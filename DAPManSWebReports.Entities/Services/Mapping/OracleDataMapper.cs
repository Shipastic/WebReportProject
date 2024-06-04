using DAPManSWebReports.Entities.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Services.Mapping
{
    public class OracleDataMapper : IDataMapper<QueryView>
    {
        private static readonly string ColumnAttributeName = "ColumnAttribute";
        public List<QueryView> MapData(IDataReader reader)
        {
            DataTable dataTable = new DataTable();
            List<QueryView> list = new List<QueryView>();
            Models.DataView dataView = new Models.DataView();
            try
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    QueryView obj = new QueryView(dataView);
                    foreach (PropertyInfo prop in typeof(QueryView).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attr = prop.GetCustomAttributes(false).FirstOrDefault(a => a.GetType().Name == ColumnAttributeName);
                        if (attr != null)
                        {
                            var columnNameProperty = attr.GetType().GetProperty("Name");
                            if (columnNameProperty != null)
                            {
                                string columnName = columnNameProperty.GetValue(attr).ToString();
                                if (!string.IsNullOrEmpty(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName)))
                                {
                                    object value = reader.GetValue(reader.GetOrdinal(columnName));
                                    if (value != null && value != DBNull.Value)
                                    {
                                        prop.SetValue(obj, Convert.ChangeType(value, prop.PropertyType));
                                    }
                                }
                            }
                        }
                    }
                    list.Add(obj);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
