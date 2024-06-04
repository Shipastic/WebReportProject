using DAPManSWebReports.Infrastructure.Interfaces;

namespace DAPManSWebReports.Infrastructure.Models
{
    public class DataSource :IDataSource
    {
        public string Name {get; set;}
        public string Description { get; set;}
        public string Provider { get; set; }
        public string Type { get; set; }
        public string Server { get; set; }
        public string DataBase { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }
        public int System { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; }
        public int Id { get; set; }
    }
}
