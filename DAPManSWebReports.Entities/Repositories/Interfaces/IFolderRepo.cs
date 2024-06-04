using DAPManSWebReports.Entities.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface IFolderRepo<T> where T : class
    {
        void InsertFavoriteFolder<T>(T obj);   
        Task<IEnumerable<T>> ReadListByMatch(string entity, string match, string orderField);
    }
}
