using DAPManSWebReports.Entities.Repositories.Implement;
using DAPManSWebReports.Entities.Repositories.Interfaces;

namespace DAPManSWebReports.Entities.Models
{
    public class DataView :IDataView
    {
        public int DataSourceId { get; set; }
        public int FolderId { get; set; }
        public string Name { get; set; }
        public string Query { get; set; }
        public string StartDateField { get; set; }
        public string StopDateField { get; set; }
        public string DataViewNote { get; set; }
        public string HiddenColumns { get; set; }
        public string ReportType { get; set; }
        public bool RequireSelection { get; set; }
        public bool MultiSelection { get; set; }
        public int ReportFormat { get; set; }
        public int TimeOut { get; set; }
        public string RemoteUser { get; set; }
        public string RemotePassword { get; set; }
        public bool IsStoredProcedure { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; }
        public int id { get; set; }
        public int ParentID { get; set; }

    }
}
