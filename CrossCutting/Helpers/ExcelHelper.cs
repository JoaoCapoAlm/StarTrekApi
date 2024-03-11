using System.Data;
using ClosedXML.Excel;
using CrossCutting.AppModel;

namespace CrossCutting.Helpers
{
    public class ExcelHelper
    {
        const string contentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static FileContent GenerateExcel(IEnumerable<DataTable> dataTables, string fileName)
        {
            fileName = string.IsNullOrWhiteSpace(fileName)
                ? $"export_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                : fileName.Trim();

            if (!fileName.EndsWith(".xlsx"))
                fileName += ".xlsx";

            using (var stream = new MemoryStream())
            {
                using (var workbook = new XLWorkbook())
                {
                    foreach (var dataTable in dataTables)
                    {
                        workbook.Worksheets.Add(dataTable).Columns().AdjustToContents();
                    }

                    workbook.SaveAs(stream);

                    stream.Position = 0;

                    var fileContent = new FileContent(stream.ToArray(), contentTypeExcel, fileName);

                    return fileContent;
                }
            }
        }
    }
}
