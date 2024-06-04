namespace DAPManSWebReports.API.Services.Paging
{
    public class PagePaging
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //public int PageCount { get; set; } = 0;
        public List<Dictionary<string, object>> ItemResult { get; set; }
        public PagePaging(List<Dictionary<string, object>> itemResult, int count, int pageNumber, int pageSize)
        {
            ItemResult = itemResult;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize  = pageSize;
        }
    }
}
