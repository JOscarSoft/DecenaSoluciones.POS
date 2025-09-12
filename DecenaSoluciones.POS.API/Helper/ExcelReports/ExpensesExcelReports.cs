using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class ExpensesExcelReports
    {
        public static void GenerateExpensesWorksheet(List<ExpensesReportViewModel> report, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateExpensesReportDataTable(report);
            var sheet = wb.Worksheets.Add(dataTable, "Gastos miselaneos");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = report.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 3, "Total");
            ReportHelper.WriteCell(sheet, totalsRow, 4, report.Sum(p => p.TotalCost));
            ReportHelper.FormatColumnAsCurrency(sheet, 4);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }
    }
}