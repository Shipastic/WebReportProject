using DAPManSWebReports.API.Services.Paging;
using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/query")]
    [ApiController]
    public class QueryViewController : ControllerBase
    {
        private IQueryViewService<QueryModel> _queryViewService;
        private IQueryParamService<QuerySettingsModel> _queryParamService;
        private readonly IMemoryCache _cache;
        public QueryViewController(IQueryViewService<QueryModel> queryViewService, IQueryParamService<QuerySettingsModel> queryParamService, IMemoryCache cache)
        {
            _queryViewService = queryViewService;
            _queryParamService = queryParamService;
            _cache = cache;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{dataviewId}")]
        public async Task<IActionResult> Get(int dataviewId, [FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            if (limit <= 0 || offset < 0)
            {
                return BadRequest("Invalid limit or offset value.");
            }
            string cacheKey = $"QueryData_{dataviewId}_FullResult";

            QuerySettingsModel settingsModel = new QuerySettingsModel();

            if (!_cache.TryGetValue(cacheKey, out QueryModel queryViewById))
            {
                settingsModel = _queryParamService.GetQueryStringParam(HttpContext);

                var queryParams = _queryParamService.GetDictionaryFromQueryString(settingsModel);
                queryViewById = await _queryViewService.GetQueryViewWithParam(dataviewId, queryParams);
                if (queryViewById?.TotalCount == 0)
                {
                    return NoContent();
                }
                // Установка кэша
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)) // пример скользящего времени жизни (опционально)
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // пример абсолютного времени жизни (опционально)

                _cache.Set(cacheKey, queryViewById, cacheEntryOptions);
            }
            //var pagedResult = PagingParametersHelper.ToPagedResult(queryViewById, settingsModel);
            var pagedItems = queryViewById.Result
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
