using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.Domain.QueryService;
using DAPManSWebReports.Entities.Repositories.Implement;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;
using LoggingLibrary.Service;

namespace DAPManSWebReports.API.Services.DI.Registration
{
    public class LoggerLibraryServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            //services.AddScoped<ILogger<OracleDBBuilder>>();
            services.AddScoped<ILogger<BaseConBuilder>>();
            //services.AddScoped<ILogger<PostgreSqlDbBuilder>>(); 
            services.AddScoped<ILogger<QueryViewRepo>>();
            services.AddScoped<ILogger<DataViewRepo>>();
            services.AddScoped<ILogger<QueryModelService>>();        }
    }
}
