using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Entities.Repositories.Interfaces
{
    public interface ISpParameter
    {
        int Direction { get; set; }
        int ParameterType { get; set; }
        string Name { get; set; }
        object Value { get; set; }
    }
}
