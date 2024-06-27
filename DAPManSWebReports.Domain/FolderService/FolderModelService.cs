using DAPManSWebReports.Domain.CommonService.CommonInterface;
using DAPManSWebReports.Domain.MappingService;
using DAPManSWebReports.Entities.Models;
using DAPManSWebReports.Entities.Repositories.Interfaces;

namespace DAPManSWebReports.Domain.FolderService
{
    public class FolderModelService : IMappingService<FolderModel>, IMenuTreeService<FolderModel>
    {
        private IBaseRepo<Folder> _baseRepo;
        public FolderModelService(IBaseRepo<Folder> baseRepo)
        {
            _baseRepo = baseRepo;
        }

        public IEnumerable<FolderModel> GetChildDtos(int parentId)
        {
            var dtoList = new List<FolderModel>();
            var folderList = _baseRepo.ReadListByParentID(parentId);
            foreach (Folder folder in folderList)
            {
                dtoList.Add(new FolderModel
                {
                    id = folder.Id,
                    Name = folder.Name,
                    Parentid = folder.ParentID,
                    Type = folder.Type,
                    LastUpdate = folder.LastUpdate,
                    LastUser = folder.LastUser,
                    Path = folder.Path,
                    RemotePassword = folder.RemotePassword,
                    RemoteUser = folder.RemoteUser,
                    System = folder.System

                });
            }
            return dtoList;
        }

        public async Task<FolderModel> GetDtoById(int id)
        {
            var folder = await _baseRepo.ReadById(id);
            if (folder == null)
                return new FolderModel();
            return new FolderModel()
            {
                id = folder.Id,
                System = folder.System,
                LastUpdate = folder.LastUpdate,
                LastUser = folder.LastUser,
                Name = folder.Name,
                Parentid = folder.ParentID,
                Path = folder.Path,
                RemotePassword = folder.RemotePassword,
                RemoteUser = folder.RemoteUser,
                Type = folder.Type
            };
        }

        public async Task<IEnumerable<FolderModel>> GetDtoList()
        {
            var dtoList = new List<FolderModel>();
            var folderList = await _baseRepo.GetAll();
            foreach (Folder folder in folderList)
            {
                dtoList.Add(new FolderModel
                {
                    id = folder.Id,
                    System = folder.System,
                    LastUpdate = folder.LastUpdate,
                    LastUser = folder.LastUser,
                    Name = folder.Name,
                    Parentid = folder.ParentID,
                    Path = folder.Path,
                    RemotePassword = folder.RemotePassword,
                    RemoteUser = folder.RemoteUser,
                    Type = folder.Type
                });
            }
            return dtoList;
        }

        public async Task<IEnumerable<FolderModel>> GetParentDtos()
        {
            var dtoList = new List<FolderModel>();
            var folderList = await _baseRepo.ReadListParentEntities();
            foreach (Folder item in folderList)
            {
                dtoList.Add(new FolderModel
                {
                    id = item.Id,
                    Name = item.Name,
                    Parentid = item.ParentID,
                    System = item.System,
                    LastUpdate = item.LastUpdate,
                    RemoteUser = item.RemoteUser,
                    RemotePassword = item.RemotePassword,
                    Path = item.Path,
                    LastUser = item.LastUser,
                    Type = item.Type
                });
            }
            return dtoList;
        }

        public Task<bool> UpdateDataAsync(FolderModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
