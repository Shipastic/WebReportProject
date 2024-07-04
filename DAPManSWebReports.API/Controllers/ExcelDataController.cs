using DAPManSWebReports.API.Services.Caching;
using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.Domain.ExcelReportService;
using DAPManSWebReports.Domain.QueryService;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DAPManSWebReports.API.Controllers
{
    [Route("api/exceldata")]
    [ApiController]
    public class ExcelDataController : ControllerBase
    {
        private readonly IQueryViewService<QueryModel> _queryViewService;
        private readonly IQueryParamService<QuerySettingsModel> _queryParamService;
        private readonly IExcelService _excelService;
        private readonly IMemoryCache _cache;
        private readonly ICacheService _cacheService;

        public ExcelDataController(IQueryViewService<QueryModel> queryViewService,
                                   IQueryParamService<QuerySettingsModel> queryParamService,
                                   IMemoryCache cache,
                                   IExcelService excelService,
                                   ICacheService cacheService)
        {
            _queryViewService = queryViewService;
            _queryParamService = queryParamService;
            _cache = cache;
            _excelService = excelService;
            _cacheService = cacheService;
        }

        [Authorize(Policy = "CustomPolicy")]
        [HttpGet("{dataviewId}")]
        public async Task<IActionResult> ExportData(int dataviewId)
        {
            GenerateCache generateCache = new GenerateCache(_cacheService);

            string cacheKey = null;

            QuerySettingsModel settingsModel = _queryParamService.GetQueryStringParam(HttpContext);

            cacheKey = generateCache.GenerateCacheKey(dataviewId, settingsModel);

            var queryViewById = await generateCache.GetQueryModelAsync(dataviewId, cacheKey, settingsModel, _queryParamService, _queryViewService);

            if (queryViewById?.TotalCount == 0)
            {
                return NoContent();
            }
            var excelData = _excelService.GenerateExcel(queryViewById.Result);

            if (excelData is not null)
            {
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
            }
            return NoContent();
        }
    }
}
