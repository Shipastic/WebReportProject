using DAPManSWebReports.API.Services.Paging;
using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/query")]
    [ApiController]
    public class QueryViewController : ControllerBase
    {
        private IQueryViewService<QueryModel> _queryViewService;
        private IQueryParamService<QuerySettingsModel> _queryParamService;
        public QueryViewController(IQueryViewService<QueryModel> queryViewService, IQueryParamService<QuerySettingsModel> queryParamService)
        {
            _queryViewService = queryViewService;
            _queryParamService = queryParamService;
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

            QuerySettingsModel settingsModel = _queryParamService.GetQueryStringParam(HttpContext);
            QueryModel queryViewById = new QueryModel();

            //if (string.IsNullOrEmpty(settingsModel.startDate) && string.IsNullOrEmpty(settingsModel.endDate))
            //{
            //    queryViewById = await _queryViewService.GetQueryView(dataviewId, limit, offset);
            //    if (queryViewById == null)
            //    {
            //        return NotFound();
            //    }
            //    return Ok(queryViewById);
            //}
            var queryParams = _queryParamService.GetDictionaryFromQueryString(settingsModel);
            queryViewById = await _queryViewService.GetQueryViewWithParam(dataviewId, queryParams);
            if (queryViewById?.TotalCount == 0)
            {
                return NoContent();
            }
            var pagedResult = PagingParametersHelper.ToPagedResult(queryViewById, settingsModel);
            var result = new
            {
                PagedItems = pagedResult.ItemResult,
                queryViewById.TotalCount,
                settingsModel.offset,
                pagedResult.PageSize,
                queryViewById.id,
                queryViewById.Name,
                queryViewById.Title,
                queryViewById.DataSourceId,
                queryViewById.Result
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
