﻿using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.Domain.QueryService;
using Microsoft.Extensions.Caching.Memory;

namespace DAPManSWebReports.API.Services.Caching
{
    internal class GenerateCache
    {
        private ICacheService _cacheService;
        public GenerateCache(ICacheService cacheService)
        {
            _cacheService = cacheService;       
        }
        public string GenerateCacheKey(int dataviewId, QuerySettingsModel settingsModel)
        {
            if (string.IsNullOrEmpty(settingsModel.startDate) && string.IsNullOrEmpty(settingsModel.stopDate))
            {
                return $"QueryData_{settingsModel.buildQuery}_{dataviewId}_FullResult";
            }
            else
            {
                return $"QueryData_{settingsModel.buildQuery}_{dataviewId}_{settingsModel.startDate}_{settingsModel.stopDate}_FullResult";
            }
        }

        public async Task<QueryModel> GetQueryModelAsync( int dataviewId, 
                                                          string cacheKey, 
                                                          QuerySettingsModel settingsModel, 
                                                          IQueryParamService<QuerySettingsModel> _queryParamService,
                                                          IQueryViewService<QueryModel> _queryViewService)
        {
            // Проверка кэша
            if (!_cacheService.TryGetValue(cacheKey, out QueryModel queryViewById))
            {
                var queryParams = _queryParamService.GetDictionaryFromQueryString(settingsModel);
                queryViewById = await _queryViewService.GetQueryViewWithParam(dataviewId, queryParams);

                if (queryViewById?.TotalCount > 0)
                {
                    _cacheService.Set(cacheKey, queryViewById);
                }
            }
            return queryViewById;
        }
    }
}
