using System.Text.Json.Serialization;

namespace DAPManSWebReports.Domain.QueryService
{
    public class QuerySettingsModel
    {
        [JsonPropertyName("dataviewId")]
        public int DataviewId { get; set; }

        [JsonPropertyName("filter")]
        public string Filter { get; set; }

        [JsonPropertyName("sortColumnNumber")]
        public string sortColumnNumber { get; set; }

        [JsonPropertyName("sortOrder")]
        public string SortOrder { get; set; }

        [JsonPropertyName("startDate")]
        public string startDate { get; set; }

        [JsonPropertyName("stopDate")]
        public string stopDate { get; set; }

        [JsonPropertyName("limit")]
        public int limit { get; set; } = 10;

        [JsonPropertyName("offset")]
        public int offset { get; set; }

        [JsonPropertyName("maxPageSize")]

        const int maxPageSize = 50;

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        [JsonPropertyName("pageSize")]
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }

    }
}
