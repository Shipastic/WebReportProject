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
        public DateTime StartDate {  get; set; }
        public DateTime StopDate {  get; set; }
        public SpParameter[] parameters {  get; set; }
        public void SetQueryParameters(Dictionary<string, object> queryParams)
        {
            if (queryParams == null)
            {
                throw new ArgumentNullException(nameof(queryParams));
            }

            parameters = queryParams.Select(param => 
            {
               if (param.Key == "startDate" || param.Key == "stopDate")
            {
                return new SpParameter
                {
                    Name = param.Key,
                    Value = Convert.ToDateTime(param.Value)
                };
            }
            else
            {
                return new SpParameter
                {
                    Name = param.Key,
                    Value = param.Value
                };
            }
        }).ToArray();
        }
    }
}