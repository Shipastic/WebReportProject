using DAPManSWebReports.Infrastructure.DbBuilder;

namespace DAPManSWebReports.Infrastructure.Interfaces
{
    public interface IBaseConBuilder
    {
        Task<BaseConBuilder> GetDbBuilder(int id);
    }
}