using DAPManSWebReports.Domain.Entities;

namespace DAPManSWebReports.API.Services.Paging
{
    public static class PagingParametersHelper
    {  public static PagePaging ToPagedResult(QueryModel list, QuerySettingsModel pagingParameters)
        {
            var totalCount = list.TotalCount;
            var items = list.Result
                .Skip((pagingParameters.offset - 1) * pagingParameters.PageSize)
                .Take(pagingParameters.PageSize)
                .ToList();

            
            return new PagePaging(items, totalCount, pagingParameters.offset, pagingParameters.PageSize);
        }     
    }
}
