using DAPManSWebReports.Entities.Repositories.Interfaces;

namespace DAPManSWebReports.Entities.Models
{
    public class Folder : Entity, IFolder
    {
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public bool System { get; set; }
        public string RemoteUser { get; set; }
        public string RemotePassword { get; set; }
        public DateTime LastUpdate { get; set; }
        public string LastUser { get; set; }
        public int Id { get; set; }
    }
}
