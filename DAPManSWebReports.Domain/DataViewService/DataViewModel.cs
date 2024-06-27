using System.Text.Json.Serialization;

namespace DAPManSWebReports.Domain.DataViewService
{
    public class DataViewModel
    {
        public int id { get; set; }

        [JsonPropertyName("dataSourceID")]
        public int DataSourceID { get; set; }

        [JsonPropertyName("folderid")]
        public int Folderid { get; set; }

        [JsonPropertyName("parentid")]
        public int Parentid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("reportType")]
        public string ReportType { get; set; }

        [JsonPropertyName("reportFormat")]
        public int ReportFormat { get; set; }

        [JsonPropertyName("remoteUser")]
        public string RemoteUser { get; set; }

        [JsonPropertyName("remotePassword")]
        public string RemotePassword { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("dataviewNote")]
        public string DataviewNote { get; set; }

        [JsonPropertyName("startDateField")]
        public string startDateField { get; set; }

        [JsonPropertyName("endDateField")]
        public string endDateField { get; set; }

    }
}
