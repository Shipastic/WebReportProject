using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.Infrastructure.DbBuilder;
using DAPManSWebReports.Infrastructure.Interfaces;

namespace DAPManSWebReports.API.Services.DI.Registration
{
    public class InfrastructureRepoServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBaseConBuilder, BaseConBuilder>();
        }
    }
}
