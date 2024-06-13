using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;

using System.Data;

namespace DAPManSWebReports.Domain.Services
{
    public class DataViewModelService : IMappingService<DataViewModel>, IMenuTreeService<DataViewModel>, IDataViewService<DataViewModel>
    {
        private IBaseRepo<DAPManSWebReports.Entities.Models.DataView> _dataViewRepository;
        private IDataViewRepo<DAPManSWebReports.Entities.Models.DataView> _viewRepo;
        public DataViewModelService(IBaseRepo<DAPManSWebReports.Entities.Models.DataView> baseRepo, IDataViewRepo<DAPManSWebReports.Entities.Models.DataView> viewRepo)
        {
            _dataViewRepository = baseRepo;
            _viewRepo = viewRepo;
        }
        public async Task<IEnumerable<DataViewModel>> GetDtoList()
        {
            var dtoList = new List<DataViewModel>();
            var dataViewList = await _dataViewRepository.GetAll();
            foreach (var dv in dataViewList)
            {
                dtoList.Add(new DataViewModel()
                {
                    DataSourceID   = dv.DataSourceId,
                    Folderid       = dv.FolderId,
                    Name           = dv.Name,
                    ReportType     = dv.ReportType,
                    RemoteUser     = dv.RemoteUser,
                    RemotePassword = dv.RemotePassword,
                    id             = dv.id,
                    Query          = dv.Query,
                    DataviewNote   = dv.DataViewNote
                });
            }
            return dtoList;
        }

        public IEnumerable<DataViewModel> GetChildDtos(int parentId)
        {
            var dtoList = new List<DataViewModel>();
            var dataViewList = _dataViewRepository.ReadListByParentID(parentId);
            foreach (var dataview in dataViewList)
            {
                dtoList.Add(new DataViewModel
                {
                    id           = dataview.id,
                    Name         = dataview.Name,
                    DataSourceID = dataview.DataSourceId,
                    Query        = dataview.Query,
                    DataviewNote = dataview.DataViewNote
                });
            }
            return dtoList;
        }

        public async Task<DataViewModel> GetDtoById(int id)
        {
            var dataViewEntity = await _dataViewRepository.ReadById(id);
            if (dataViewEntity == null)
                return new DataViewModel();
            return new DataViewModel()
            {
                DataSourceID   = dataViewEntity.DataSourceId,
                Folderid       = dataViewEntity.FolderId,
                Name           = dataViewEntity.Name,
                ReportType     = dataViewEntity.ReportType,
                RemoteUser     = dataViewEntity.RemoteUser,
                RemotePassword = dataViewEntity.RemotePassword,
                id             = dataViewEntity.id,
                Query          = dataViewEntity.Query,
                DataviewNote   = dataViewEntity.DataViewNote,
                startDateField = dataViewEntity.StartDateField,
                endDateField   = dataViewEntity.StopDateField
            };
        }

        public async Task<IEnumerable<DataViewModel>> GetParentDtos()
        {
            var dtoList = new List<DataViewModel>();
            var dataviewList = await _dataViewRepository.ReadListParentEntities();
            foreach (var item in dataviewList)
            {
                dtoList.Add(new DataViewModel
                {
                    id           = item.id,
                    Name         = item.Name,
                    Query        = item.Query,
                    DataSourceID = item.DataSourceId,
                    DataviewNote = item.DataViewNote
                });
            }
            return dtoList;
        }

        public async Task<IEnumerable<DataViewModel>> GetParentDtosFromList(List<int> ids)
        {
            var dtoList = new List<DataViewModel>();
            var dataviewList = await _viewRepo.ReadListFromFolderList(ids);
            foreach (var item in dataviewList)
            {
                dtoList.Add(new DataViewModel
                {
                    id           = item.id,
                    Name         = item.Name,
                    Query        = item.Query,
                    DataSourceID = item.DataSourceId,
                    Folderid     = item.FolderId,
                    DataviewNote = item.DataViewNote
                });
            }
            return dtoList;

        }
    }
}
