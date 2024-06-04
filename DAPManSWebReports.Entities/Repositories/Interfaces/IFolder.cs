namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface IFolder
    {
        int Id { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        string Name { get; set; }
        int ParentID { get; set; }
        string Path { get; set; }
        string RemotePassword { get; set; }
        string RemoteUser { get; set; }
        bool System { get; set; }
        string Type { get; set; }
    }
}