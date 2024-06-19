using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class QuerySettingsModel
    {
        public int DataviewId { get; set; }
        public string Filter { get; set; }
        public string sortColumnNumber { get; set; }
        public string SortOrder { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int limit { get; set; } = 10;
        public int offset { get; set; }

        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

    }
}
