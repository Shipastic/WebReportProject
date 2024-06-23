using DAPManSWebReports.Entities.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface IDataViewRepo<T> where T : class
    {
        Task<IEnumerable<T>> ReadListFromFolderList(List<int> folderListId);
    }
}
