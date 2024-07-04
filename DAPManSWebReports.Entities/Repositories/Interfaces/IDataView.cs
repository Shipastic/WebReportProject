using DAPManSWebReports.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface IDataView
    {
        int DataSourceId { get; set; }
        int FolderId { get; set; }
        string Name { get; set; }
        string Query { get; set; }
        string StartDateField { get; set; }
        string StopDateField { get; set; }
        string DataViewNote { get; set; }
        string HiddenColumns { get; set; }
        string ReportType { get; set; }
        bool RequireSelection { get; set; }
        bool MultiSelection { get; set; }
        int ReportFormat { get; set; }
        int TimeOut { get; set; }
        string RemoteUser { get; set; }
        string RemotePassword { get; set; }
        bool IsStoredProcedure { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        SpParameter[] parameters {  get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime StopDate {  get; set; }

    }
}
