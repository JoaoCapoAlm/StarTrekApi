using System.Data;
using ClosedXML.Excel;
using CrossCutting.AppModel;

namespace CrossCutting.Helpers
{
    public class ExcelHelper
    {
        private readonly static string CONTENT_TYPE_EXCEL = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static FileContent GenerateExcel(IEnumerable<DataTable> dataTables, string fileName)
        {
            fileName = string.IsNullOrWhiteSpace(fileName)
                ? $"export_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                : fileName.Trim();

            if (!fileName.EndsWith(".xlsx"))
                fileName += ".xlsx";
            
            using var workbook = new XLWorkbook();
            foreach (var dataTable in dataTables)
            {
                var ws = workbook.Worksheets.Add(dataTable);
                ws.Columns().AdjustToContents();
                ws.SheetView.FreezeRows(1);
                ws.FirstColumn().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            workbook.Dispose();

            stream.Position = 0;

            var fileContent = new FileContent(stream.ToArray(), CONTENT_TYPE_EXCEL, fileName);
            stream.DisposeAsync();

            return fileContent;
        }
    }
}
