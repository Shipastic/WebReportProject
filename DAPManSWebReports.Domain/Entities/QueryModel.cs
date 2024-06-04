using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class QueryModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int DataSourceId { get; set; }
        public string Title { get; set; }
        public int TotalCount {  get; set; }
        //public DataTable Result { get; set; }
        public List<Dictionary<string, object>> Result { get; set; }
    }
}
