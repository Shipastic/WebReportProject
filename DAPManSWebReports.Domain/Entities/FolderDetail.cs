using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class FolderDetail
    {
        [JsonPropertyName("folderId")]
        public int FolderId { get; set; }

        [JsonPropertyName("folderName")]
        public string FolderName { get; set; }

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("childFolders")]
        public IEnumerable<FolderModel> ChildFolders { get; set; }

        [JsonPropertyName("dataviews")]
        public IEnumerable<DataViewModel> Dataviews { get; set; }
    }
}
