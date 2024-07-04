using DAPManSWebReports.Domain.QueryService;

using Microsoft.Extensions.Primitives;

using System.ComponentModel;
using System.Reflection;

namespace DAPManSWebReports.API.Services.QueryParamService
{
    public class QueryParamServices : IQueryParamService<QuerySettingsModel>
    {
        public QuerySettingsModel GetQueryStringParam(HttpContext context)
        {
            QuerySettingsModel querySettingsModel = new QuerySettingsModel();
            var queries = context.Request.Query;

            // Получаем все свойства класса
            var properties = typeof(QuerySettingsModel).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanWrite);

            foreach (var property in properties)
            {
                if (queries.TryGetValue(property.Name, out StringValues value))
                {
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);

                    if (converter != null && converter.CanConvertFrom(typeof(string)))
                    {
                        try
                        {
                            var convertedValue = converter.ConvertFromInvariantString(value.FirstOrDefault());
                            property.SetValue(querySettingsModel, convertedValue);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            return querySettingsModel;
        }

        public Dictionary<string, object> GetDictionaryFromQueryString(QuerySettingsModel querySettings)
        {
            var dictionary = new Dictionary<string, object>();

            var properties = typeof(QuerySettingsModel).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                        .Where(p => p.CanRead);
            foreach (var property in properties)
            {
                var value = property.GetValue(querySettings);
                dictionary.Add(property.Name, value);
            }

            return dictionary;
        }
    }
}
