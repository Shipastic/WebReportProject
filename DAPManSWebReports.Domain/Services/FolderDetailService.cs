using DAPManSWebReports.Domain.Entities;
using DAPManSWebReports.Domain.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Services
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
