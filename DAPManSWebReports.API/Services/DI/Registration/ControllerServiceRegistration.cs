using DAPManSWebReports.API.Services.Caching;
using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.API.Services.QueryParamService;
using DAPManSWebReports.API.Services.Sorting;
using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;
using DAPManSWebReports.Domain.Services;

namespace DAPManSWebReports.API.Services.DI.Registration
{
    public class ControllerServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IQueryParamService<QuerySettingsModel>, QueryParamServices>();

            services.AddScoped<ICacheService, CacheService>();

            services.AddScoped<ISortingService, SortingService>();
        }
    }
}
