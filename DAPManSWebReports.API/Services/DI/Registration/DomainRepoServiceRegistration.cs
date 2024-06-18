using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;
using DAPManSWebReports.Domain.Services;

namespace DAPManSWebReports.API.Services.DI.Registration
{
    public class DomainRepoServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IMappingService<DataViewModel>, DataViewModelService>();
            services.AddScoped<IMappingService<FolderModel>,   FolderModelService>();

            services.AddScoped<IMenuTreeService<FolderModel>,   FolderModelService>();
            services.AddScoped<IMenuTreeService<DataViewModel>, DataViewModelService>();

            services.AddScoped<IMenuService<FolderDetail, FolderModel, DataViewModel>, FolderDetailService>();

            services.AddScoped<IQueryViewService<QueryModel>, QueryModelService>();

            services.AddScoped<IDataViewService<DataViewModel>, DataViewModelService>();

            services.AddScoped<IExcelService, ExcelService> ();
        }
    }
}
