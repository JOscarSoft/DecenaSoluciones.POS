using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;
using System.Data;
using System.Globalization;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class SalesExcelReports
    {
        public static void GetSalesAndInventoryResumeWorkSheet(
            InventoryReportViewModel inventoryReport,
            List<SalesReportViewModel> salesReport,
            List<ExpensesReportViewModel> miscellaneousExpenses,
            XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet("Resumen");


            decimal inCost = inventoryReport.inventoryInEntriesDetails.Sum(p => p.TotalCost);
            decimal outCost = inventoryReport.inventoryOutEntriesDetails.Sum(p => p.TotalCost);
            decimal salesTotal = salesReport.Sum(p => p.Total!.Value);
            decimal miscTotal = miscellaneousExpenses.Sum(p => p.TotalCost);
            decimal netProfit = salesTotal - (inCost + outCost + miscTotal);
            sheet1.Cell(2, 2).Value = $"Resumen de ingresos/Egresos desde {inventoryReport.From!.Value:dd/MM/yyyy} hasta {inventoryReport.To!.Value:dd/MM/yyyy}";

            sheet1.Cell(3, 2).Value = "Productos ingresados";
            sheet1.Cell(3, 3).Value = inventoryReport.inventoryInEntriesDetails.Sum(p => p.Quantity).ToString().Replace(".00", "");

            sheet1.Cell(4, 2).Value = "Costo total de productos ingresados";
            sheet1.Cell(4, 3).Value = inCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(4, 3).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(4, 3).Style.Font.Bold = true;
            sheet1.Cell(4, 3).Style.Font.FontColor = XLColor.Red;

            sheet1.Cell(5, 2).Value = "Productos retirados";
            sheet1.Cell(5, 3).Value = inventoryReport.inventoryOutEntriesDetails.Sum(p => p.Quantity).ToString().Replace(".00", "");

            sheet1.Cell(6, 2).Value = "Costo total de productos retirados";
            sheet1.Cell(6, 3).Value = outCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(6, 3).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(6, 3).Style.Font.Bold = true;
            sheet1.Cell(6, 3).Style.Font.FontColor = XLColor.Red;

            sheet1.Cell(7, 2).Value = "Gastos miscelaneos";
            sheet1.Cell(7, 3).Value = miscellaneousExpenses.Count.ToString();

            sheet1.Cell(8, 2).Value = "Costo total de miscelaneos";
            sheet1.Cell(8, 3).Value = miscTotal.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(8, 3).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(8, 3).Style.Font.Bold = true;
            sheet1.Cell(8, 3).Style.Font.FontColor = XLColor.Red;

            sheet1.Cell(9, 2).Value = "Productos vendidos";
            sheet1.Cell(9, 3).Value = salesReport.Sum(p => p.ProductsQuantity).ToString().Replace(".00", "");

            sheet1.Cell(10, 2).Value = "Total de ingresos por ventas";
            sheet1.Cell(10, 3).Value = salesTotal.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(10, 3).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(10, 3).Style.Font.Bold = true;
            sheet1.Cell(10, 3).Style.Font.FontColor = XLColor.Green;

            sheet1.Cell(11, 2).Value = "Ganancia neta (ventas - (gastos + costo de productos))";
            sheet1.Cell(11, 3).Value = netProfit.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(11, 3).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(11, 3).Style.Font.FontColor = netProfit <= 0 ? XLColor.Red : XLColor.Green;
            sheet1.Cell(11, 3).Style.Font.Bold = true;


            sheet1.Range(sheet1.Cell(2, 2), sheet1.Cell(2, 3)).Merge();
            sheet1.Range(sheet1.Cell(3, 2), sheet1.Cell(11, 3)).Style.Font.FontSize = 13;
            sheet1.Cell(11, 3).Style.Font.FontSize = 14;
            sheet1.Row(2).CellsUsed().Style.Fill.BackgroundColor = XLColor.AliceBlue;
            sheet1.Row(2).Style.Font.Bold = true;
            sheet1.Row(2).Style.Font.Shadow = true;
            sheet1.Row(2).Style.Font.FontSize = 16;

            for (int i = 3; i <= 11; i++)
            {
                sheet1.Cell(i, 2).Style.Font.Bold = true;
                sheet1.Cell(i, 2).Style.Font.Shadow = true;
                sheet1.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.AliceBlue;
            }

            sheet1.Columns(1, 1).Width = 5;
            sheet1.Columns(2, 2).Width = 60;
            sheet1.Columns(3, 3).Width = 30;
            sheet1.Range(sheet1.Cell(2, 2), sheet1.Cell(11, 3)).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            sheet1.Range(sheet1.Cell(2, 2), sheet1.Cell(11, 3)).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
        }

        public static void GetSoldProductExcelReport(List<SoldProductsReportViewModel> report, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetSoldProductsReportDataTable(report), "Ventas por productos");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;

            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(report.Count() + 2).Cell(2).Value = "Totales";
            sheet1.Row(report.Count() + 2).Cell(3).Value = report.Sum(p => p.Quantity);
            sheet1.Row(report.Count() + 2).Cell(4).Value = report.Sum(p => p.SalesTotal);
            sheet1.Row(report.Count() + 2).Cell(5).Value = report.Sum(p => p.Stock);
            sheet1.Row(report.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(report.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 4).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }

        public static void GetSalesReportWorkSheet(List<SalesReportViewModel> report, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetSalesReportDataTable(report), "Reporte de ventas");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(report.Count() + 2).Cell(4).Value = "Totales";
            sheet1.Row(report.Count() + 2).Cell(5).Value = report.Sum(p => p.ProductsQuantity);
            sheet1.Row(report.Count() + 2).Cell(6).Value = report.Sum(p => p.Total);
            sheet1.Row(report.Count() + 2).Cell(7).Value = report.Sum(p => p.PayedAmount);
            sheet1.Row(report.Count() + 2).Cell(8).Value = report.Sum(p => p.Devolution);
            sheet1.Row(report.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(report.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 6).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 7).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 8).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }
        private static DataTable GetSoldProductsReportDataTable(List<SoldProductsReportViewModel> sales)
        {
            var dt = new DataTable();
            dt.TableName = "SoldProductsReport";
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));

            if (sales.Count > 0)
            {
                sales.OrderBy(p => p.Description).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.Code, item.Description, item.Quantity, item.SalesTotal, item.Stock);
                });
            }

            return dt;
        }

        private static DataTable GetSalesReportDataTable(List<SalesReportViewModel> sales)
        {
            var dt = new DataTable();
            dt.TableName = "SalesReport";
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateOnly));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Productos", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Monto pagado", typeof(decimal));
            dt.Columns.Add("Devuelta", typeof(decimal));

            if (sales.Count > 0)
            {
                sales.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.Code, item.CustomerName, DateOnly.FromDateTime(item.CreationDate), item.UserName, item.ProductsQuantity, item.Total, item.PayedAmount, item.Devolution);
                });
            }

            return dt;
        }
    }
}
