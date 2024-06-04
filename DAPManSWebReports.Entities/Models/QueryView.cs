using System.Data;

namespace DAPManSWebReports.Entities.Models
{
    public class QueryView
    {
        private int datasourceid;
        private int id;
        private string name;
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (DataView == null)
                {
                    throw new InvalidOperationException("DataView is not initialized.");
                }
                title = DataView.Name;
            }
        }
        public int Id 
        { 
            get
            {
                return id;
            }
            set
            {
                if (DataView == null)
                {
                    throw new InvalidOperationException("DataView is not initialized.");
                }
                id = DataView.id;
            }
        }
        public string? Name 
        { 
            get 
            {
                return name;
            } 
            set 
            {
                if (DataView == null)
                {
                    throw new InvalidOperationException("DataView is not initialized.");
                }
                name = DataView.Query;
            } 
        }
        public DataTable TableResultQuery { get; set; }
        public int DatasourceId
        {
            get
            {
                return datasourceid;
            }
            set
            {
                if (DataView == null)
                {
                    throw new InvalidOperationException("DataView is not initialized.");
                }
                datasourceid = DataView.DataSourceId;
            }
        }
        public DataView? DataView { get; set; } = null;
        public int TotalCount { get;set; }
        public QueryView(DataView dataView)
        {
            DataView = dataView;

            if (DataView != null)
            {
                datasourceid = DataView.DataSourceId;
                id           = DataView.id;
                name         = DataView.Query ?? string.Empty;
                title        = DataView.Name ?? string.Empty;
            }
            TableResultQuery = new DataTable();
        }
    }
}
