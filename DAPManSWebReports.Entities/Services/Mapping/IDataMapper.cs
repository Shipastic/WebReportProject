using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Services.Mapping
{
    public interface IDataMapper<T> where T : class
    {
       List<T> MapData(IDataReader reader);
    }
}
