using DAPManSWebReports.Domain.Interfaces;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

namespace DAPManSWebReports.Domain.Services
{
    public class ExcelService : IExcelService
    {
        public byte[] GenerateExcel(List<Dictionary<string, object>> data)
        {
            // Создаем новый workbook 
            IWorkbook workbook = new XSSFWorkbook();
            // Создаем новый лист
            ISheet sheet = workbook.CreateSheet("Sheet1");

            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("The data list is empty or null");
            }

            var headers = data[0].Keys;
            // Создаем строку для заголовков (первая строка)
            IRow headerRow = sheet.CreateRow(0);
            int colIndex = 0;
            foreach (var header in headers)
            {
                // Добавляем заголовки в первую строку
                headerRow.CreateCell(colIndex).SetCellValue(header);
                colIndex++;
            }

            colIndex = FillData(data, sheet, headers, colIndex);

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                return ms.ToArray();
            }
        }

        private static int FillData(List<Dictionary<string, object>> data, ISheet sheet, Dictionary<string, object>.KeyCollection headers, int colIndex)
        {
            // Заполняем данными
            for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
            {
                var row = data[rowIndex];
                // Создаем новую строку для каждого набора данных
                IRow excelRow = sheet.CreateRow(rowIndex + 1);  
                colIndex = 0;

                
                    foreach (var header in headers)
                    {
                        var cellValue = row[header];
                    try
                    {
                        // Обработка null значений
                        if (cellValue is null || cellValue is Dictionary<string, object> emptyDict && emptyDict.Count == 0)
                        {
                            excelRow.CreateCell(colIndex).SetCellValue(string.Empty);
                        }
                        else
                        {
                            switch (cellValue)
                            {
                                case int:
                                    excelRow.CreateCell(colIndex).SetCellValue((int)cellValue);
                                    break;
                                case double:
                                    excelRow.CreateCell(colIndex).SetCellValue((double)cellValue);
                                    break;
                                case bool:
                                    excelRow.CreateCell(colIndex).SetCellValue((bool)cellValue);
                                    break;
                                case DateTime:
                                    excelRow.CreateCell(colIndex).SetCellValue(((DateTime)cellValue).ToString("yyyy-MM-dd HH:mm:ss"));
                                    break;
                                default:
                                    excelRow.CreateCell(colIndex).SetCellValue(cellValue.ToString());
                                    break;
                            }
                            colIndex++;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{e.Message}\n{excelRow}\n{cellValue}\n{row}");
                    }
                }
               
            }

            return colIndex;
        }

        public void SaveFile(string filePath, byte[] fileBytes)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(fileBytes, 0, fileBytes.Length);
            }
        }
    }
}
