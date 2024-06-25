using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class QueryModel
    {
        public int id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("dataSourceId")]
        public int DataSourceId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount {  get; set; }

        [JsonPropertyName("queryResult")]
        public string QueryResult { get; set; }

        [JsonPropertyName("result")]
        public List<Dictionary<string, object>> Result { get; set; }
    }
}
