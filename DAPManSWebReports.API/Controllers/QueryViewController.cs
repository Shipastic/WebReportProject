using DAPManSWebReports.API.Services.Caching;
using DAPManSWebReports.API.Services.Paging;
using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.API.Services.Sorting;
using DAPManSWebReports.Domain.ExcelReportService;
using DAPManSWebReports.Domain.QueryService;
using LoggingLibrary.Service;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/query")]
    [ApiController]
    public class QueryViewController : ControllerBase
    {
        private readonly IQueryViewService<QueryModel> _queryViewService;
        private readonly IQueryParamService<QuerySettingsModel> _queryParamService;
        private readonly IExcelService _excelService;
        private readonly IMemoryCache _cache;
        private readonly ICacheService _cacheService;
        private readonly ISortingService _sortingService;
        private  readonly ILogger<QueryViewController> _logger;
        public QueryViewController(IQueryViewService<QueryModel> queryViewService, 
                                   IQueryParamService<QuerySettingsModel> queryParamService, 
                                   IMemoryCache cache,
                                   IExcelService excelService,
                                   ICacheService cacheService,
                                   ISortingService sortingService,
                                   ILogger<QueryViewController> logger)
        {
            _queryViewService = queryViewService;
            _queryParamService = queryParamService;
            _cache = cache;
            _excelService = excelService;
            _cacheService = cacheService;
            _sortingService = sortingService;
            _logger = logger;
        }
        [Authorize]
        [HttpGet("{dataviewId}")]
        public async Task<IActionResult> Get(int dataviewId, 
                                             [FromQuery] int limit = 10, 
                                             [FromQuery] int offset = 0,
                                             [FromQuery] string sortColumn = null)
        {
            if (limit <= 0 || offset < 0)
            {
                return BadRequest("Invalid limit or offset value.");
            }
            GenerateCache generateCache = new GenerateCache(_cacheService);

            string cacheKey = null;

            QuerySettingsModel settingsModel = _queryParamService.GetQueryStringParam(HttpContext);
            //для лога
            _logger.LogInformation($"{DateTime.Now}|\tShow params query:",ConsoleColor.Blue);
            var queryCollection = HttpContext.Request.Query;
            foreach (var queryParameter in queryCollection)
            {
                var key = queryParameter.Key;
                var values = string.Join(",", queryParameter.Value);
                _logger.LogInformation($"{DateTime.Now}|\t Query params:\n{key} - {values}\n");
            }
            //
            cacheKey = generateCache.GenerateCacheKey(dataviewId, settingsModel);

            var queryViewById = await generateCache.GetQueryModelAsync(dataviewId, cacheKey, settingsModel, _queryParamService, _queryViewService);

            if (queryViewById?.TotalCount == 0)
            {
                _logger.LogError($"{DateTime.Now}|\t queryViewById:{queryViewById}| TotalCount = {queryViewById?.TotalCount}");
                return NoContent();              
            }

            var sortedResult = _sortingService
                                        .ApplySorting(queryViewById.Result.AsQueryable(), settingsModel.sortColumnNumber, settingsModel.SortOrder);

            _logger.LogInformation($"{DateTime.Now}|\t Column name for sorting:{settingsModel.sortColumnNumber}| Sort order:{settingsModel.SortOrder}");
            var pagedItems = sortedResult
                                         .Skip(offset)
                                         .Take(limit)
                                         .ToList();
            var result = new
            {
                pagedItems,
                queryViewById.TotalCount,
                offset,
                PageSize = limit,
                queryViewById.id,
                queryViewById.Name,
                queryViewById.Title,
                queryViewById.DataSourceId,
                queryViewById.Result,
                queryViewById.QueryResult
            };
            _logger.LogInformation($"{DateTime.Now}|\t  ID:{queryViewById.id}| Name:{queryViewById.Name}| Title:{queryViewById.Title}| TotalCount:{queryViewById.TotalCount} | limit:{limit}| Query Result:{queryViewById.QueryResult}");
            return Ok(result);
        }

        // POST api/<QueryViewController>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return StatusCode(501);
        }

        // PUT api/<QueryViewController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(501);
        }

        // DELETE api/<QueryViewController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return StatusCode(501);
        }
    }
}
