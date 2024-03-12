using System.Data;
using ClosedXML.Excel;
using CrossCutting.AppModel;

namespace CrossCutting.Helpers
{
    public class ExcelHelper
    {
        private const string contentTypeExcel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
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
                        var ws = workbook.Worksheets.Add(dataTable);
                        ws.Columns().AdjustToContents();
                        ws.FirstColumn().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        ws.FirstColumn().Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        var cells = ws.CellsUsed(x => x.DataType.Equals(XLDataType.Number)).ToArray();
                        foreach (var cell in cells)
                        {
                            cell.Style.NumberFormat.SetNumberFormatId(XLPredefinedFormat.Number.Integer.GetHashCode());
                        }
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
