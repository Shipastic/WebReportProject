namespace DAPManSWebReports.Domain.ExcelReportService
{
    public interface IExcelService
    {
        byte[] GenerateExcel(List<Dictionary<string, object>> data);
        void SaveFile(string filePath, byte[] fileBytes);
    }
}
