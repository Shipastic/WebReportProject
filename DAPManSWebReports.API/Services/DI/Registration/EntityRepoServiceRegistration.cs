using DAPManSWebReports.API.Services.DI.Interfaces;
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Implement;
using DAPManSWebReports.Entities.Repositories.Interfaces;

using Microsoft.AspNetCore.Components.Forms;

namespace DAPManSWebReports.API.Services.DI.Registration
{
    public class EntityRepoServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IBaseRepo<DataView>, DataViewRepo>();
            services.AddScoped<IBaseRepo<Folder>,   FolderRepo>();

            services.AddScoped<IFolderRepo<Folder>, FolderRepo>();

            services.AddScoped<IQueryRepo<QueryView>, QueryViewRepo>();

            services.AddScoped<IDataViewRepo<DataView>, DataViewRepo>();
        }
    }
}
