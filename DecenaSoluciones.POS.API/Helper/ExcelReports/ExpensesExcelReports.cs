using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;
using System.Data;
using System.Globalization;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class ExpensesExcelReports
    {
        public static void GetExpensesWorkSheet(List<ExpensesReportViewModel> report, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetExpensesReportDataTable(report), "Gastos miselaneos");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;

            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(report.Count() + 2).Cell(3).Value = "Total";
            sheet1.Row(report.Count() + 2).Cell(4).Value = report.Sum(p => p.TotalCost);
            sheet1.Row(report.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(report.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 4).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }
        private static DataTable GetExpensesReportDataTable(List<ExpensesReportViewModel> expenses)
        {
            var dt = new DataTable();
            dt.TableName = "MiscellaneousExpensesReport";
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateOnly));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Monto", typeof(decimal));

            if (expenses.Count > 0)
            {
                expenses.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.UserName, DateOnly.FromDateTime(item.CreationDate), item.Comments, item.TotalCost);
                });
            }

            return dt;
        }
    }
}
