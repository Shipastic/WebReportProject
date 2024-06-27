using DAPManSWebReports.Domain.DataViewService;

using System.Text.Json.Serialization;

namespace DAPManSWebReports.Domain.FolderService
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
