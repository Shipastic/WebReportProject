using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Entities
{
    public class FolderModel
    {
        public int id { get; set; }
        public int Parentid { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public bool System { get; set; }
        public string RemoteUser { get; set; }
        public string RemotePassword { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; }
        
    }
}
