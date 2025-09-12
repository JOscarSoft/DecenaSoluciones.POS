using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class ProductsExcelReports
    {
        public static void GenerateProductsExcelWorksheet(List<ProductsReportViewModel> result, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateProductsReportDataTable(result);
            var sheet = wb.Worksheets.Add(dataTable, "Reporte de productos");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = result.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 4, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 5, result.Sum(p => p.stock));
            ReportHelper.WriteCell(sheet, totalsRow, 6, result.Sum(p => p.TotalCost));
            ReportHelper.WriteCell(sheet, totalsRow, 7, result.Sum(p => p.TotalPrice));
            ReportHelper.WriteCell(sheet, totalsRow, 8, result.Sum(p => p.Revenue));

            ReportHelper.FormatColumnAsCurrency(sheet, 3);
            ReportHelper.FormatColumnAsCurrency(sheet, 4);
            ReportHelper.FormatColumnAsCurrency(sheet, 6);
            ReportHelper.FormatColumnAsCurrency(sheet, 7);
            ReportHelper.FormatColumnAsCurrency(sheet, 8);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }
    }
}