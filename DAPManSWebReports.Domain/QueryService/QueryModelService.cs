using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.QueryService
{
    public class QueryModelService : IQueryViewService<QueryModel>
    {
        private IQueryRepo<QueryView> _queryRepository;

        private readonly ILogger<QueryModelService> _logger;
        public QueryModelService(IQueryRepo<QueryView> queryRepository, ILogger<QueryModelService> logger)
        {
            _queryRepository = queryRepository;
            _logger = logger;
        }

        public async Task<QueryModel> GetQueryView(int dataviewId, int limit = 10, int offset = 0)
        {
            var queryView = await _queryRepository.ReadById(dataviewId, limit, offset);
            return new QueryModel
            {
                Name = queryView.Name,
                DataSourceId = queryView.DatasourceId,
                Result = ConvertDataTableToList(queryView.TableResultQuery),
                id = queryView.Id,
                Title = queryView.Title
            };
        }

        public async Task<QueryModel> GetQueryViewWithParam(int dataviewId, Dictionary<string, object> queryparams)
        {
            _logger.LogInformation($"{DateTime.Now}|\t GetQueryViewWithParam dataviewId -{dataviewId}");
            var queryView = await _queryRepository.ReadById(dataviewId, queryparams);
            return new QueryModel
            {
                Name = queryView.Name,
                DataSourceId = queryView.DatasourceId,
                Result = ConvertDataTableToList(queryView.TableResultQuery),
                id = queryView.Id,
                Title = queryView.Title,
                TotalCount = queryView.TotalCount,
                QueryResult = queryView.ResultQuery
            };
        }

        private List<Dictionary<string, object>> ConvertDataTableToList(DataTable dt)
        {
            var columns = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();

            var result = dt.AsEnumerable().Select(row =>
            {
                var dictionary = columns.ToDictionary(column => column,
                                                       column => row[column] is DBNull ? null : row[column]);
                // Проверка на пустой объект
                foreach (var key in dictionary.Keys.ToList())
                {
                    if (dictionary[key] is Dictionary<string, object> dict && dict.Count == 0)
                    {
                        dictionary[key] = null;
                    }
                }

                return dictionary;
            }).ToList();

            return result;
        }
    }
}
