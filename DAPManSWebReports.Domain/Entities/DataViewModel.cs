using DAPManSWebReports.Domain.Interfaces; 
using DAPManSWebReports.Entities.Repositories.Interfaces;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class DataViewModel
    {
        public int id { get; set; }
        public int DataSourceID { get; set; }
        public int Folderid { get; set; }
        public int Parentid { get; set; }
        public string Name { get; set; }
        public string ReportType { get; set; }
        public int ReportFormat { get; set; }
        public string RemoteUser { get; set; }
        public string RemotePassword { get; set; }
        public string Query { get; set; }
        public string DataviewNote { get; set; }
       
    }
}
