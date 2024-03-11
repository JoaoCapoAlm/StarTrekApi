using System.Data;
using ClosedXML.Excel;
using CrossCutting.AppModel;

namespace CrossCutting.Helpers
{
    public class ExcelHelper
    {
        public static FileContent GenerateExcel(DataTable dataTable, string excelName)
        {
            using (var stream = new MemoryStream())
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(dataTable);
                    workbook.SaveAs(stream);

                    stream.Position = 0;

                    var fileContent = new FileContent()
                    {
                        Content = stream.ToArray(),
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileDownloadName = excelName
                    };

                    return fileContent;
                }
            }
        }
    }
}
