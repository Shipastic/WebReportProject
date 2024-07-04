using DAPManSWebReports.Infrastructure.DbBuilder;

namespace DAPManSWebReports.Infrastructure.Interfaces
{
    public interface IBaseConBuilder
    {
        Task<BaseConBuilder> GetDbBuilder(int id);
        /// <summary>
        /// Получаем тип БД по DataSourceId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetTypeDb(int id);
    }
}