﻿using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Services
{
    public class QueryModelService : IQueryViewService<QueryModel>
    {
        private IQueryRepo<QueryView> _queryRepository;

        public QueryModelService(IQueryRepo<QueryView> queryRepository)
        {
            _queryRepository = queryRepository;
        }

        public async Task<QueryModel> GetQueryView(int dataviewId, int limit = 10, int offset = 0)
        {         
            var queryView =await _queryRepository.ReadById(dataviewId, limit, offset);
            return new QueryModel 
                        { Name         = queryView.Name, 
                          DataSourceId = queryView.DatasourceId, 
                          Result       = ConvertDataTableToList(queryView.TableResultQuery),
                          id           = queryView.Id,
                          Title        = queryView.Title
            };
        }

        public async Task<QueryModel> GetQueryViewWithParam(int dataviewId, Dictionary<string, object> queryparams)
        {
            var queryView    = await _queryRepository.ReadById(dataviewId, queryparams);
            return new QueryModel
            {
                Name         = queryView.Name,
                DataSourceId = queryView.DatasourceId,
                Result       = ConvertDataTableToList(queryView.TableResultQuery),
                id           = queryView.Id,
                Title        = queryView.Title,
                TotalCount   = queryView.TotalCount,
                QueryResult  = queryView.ResultQuery
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
