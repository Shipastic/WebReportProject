using DAPManSWebReports.Domain.DataViewService;

namespace DAPManSWebReports.Domain.FolderService
{
    public class FolderDetailService : IMenuService<FolderDetail, FolderModel, DataViewModel>
    {
        public async Task<FolderDetail> GetListDtos(IEnumerable<FolderModel> folderDtoList, IEnumerable<DataViewModel> dataViewDtoList)
        {
            var entityDTOs = new FolderDetail
            {
                ChildFolders = folderDtoList,
                Dataviews = dataViewDtoList
            };

            return entityDTOs;
        }
    }
}
