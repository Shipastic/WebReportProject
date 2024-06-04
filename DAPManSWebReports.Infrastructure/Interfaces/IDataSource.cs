namespace DAPManSWebReports.Infrastructure.Interfaces
{
    public interface IDataSource
    {
        string Name { get; set; }
        string Description { get; set; }
        string Provider { get; set; }
        string Type { get; set; }
        string Server { get; set; }
        string DataBase { get; set; }
        string DbUser { get; set; }
        string DbPassword { get; set; }
        int System { get; set; }
        DateTime LastUpdate { get; set; }
        string LastUser { get; set; }
        int Id { get; set; }
    }
}
