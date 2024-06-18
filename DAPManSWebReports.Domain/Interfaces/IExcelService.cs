using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Interfaces
{
    public interface IExcelService
    {
        byte[] GenerateExcel(List<Dictionary<string, object>> data);
        void SaveFile(string filePath, byte[] fileBytes);
    }
}
