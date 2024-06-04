using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Interfaces
{
    public interface IDataViewService<T> where T : class
    {
        Task<IEnumerable<T>> GetParentDtosFromList(List<int> ids);
    }
}
