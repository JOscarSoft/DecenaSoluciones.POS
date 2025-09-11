using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;
using System.Data;
using System.Globalization;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class SalesExcelReports
    {
        private static readonly CultureInfo UsCulture = CultureInfo.CreateSpecificCulture("en-US");
        private const string CurrencyFormat = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";

        public static void GetSalesAndInventoryResumeWorkSheet(
            InventoryReportViewModel inventoryReport,
            List<SalesReportViewModel> salesReport,
            List<ExpensesReportViewModel> miscellaneousExpenses,
            List<ProductsReportViewModel> productsReport,
            XLWorkbook workbook)
        {
            var sheet = workbook.AddWorksheet("Resumen");

            decimal totalIngresoInventario = inventoryReport.inventoryInEntriesDetails.Sum(p => p.TotalCost);
            decimal totalSalidaInventario = inventoryReport.inventoryOutEntriesDetails.Sum(p => p.TotalCost);
            decimal totalVentas = salesReport.Sum(p => p.Total!.Value);
            decimal totalGastosMiscelaneos = miscellaneousExpenses.Sum(p => p.TotalCost);
            decimal gananciaNeta = totalVentas - (totalIngresoInventario + totalSalidaInventario + totalGastosMiscelaneos);

            BuildResumenIngresosEgresosSection(sheet, inventoryReport, totalIngresoInventario, totalSalidaInventario, totalVentas, totalGastosMiscelaneos, gananciaNeta, salesReport, miscellaneousExpenses);
            BuildResumenProductosSection(sheet, productsReport);

            ApplyGeneralFormatting(sheet);
        }

        private static void BuildResumenIngresosEgresosSection(IXLWorksheet sheet, InventoryReportViewModel inventoryReport,
            decimal inCost, decimal outCost, decimal salesTotal, decimal miscTotal, decimal netProfit,
            List<SalesReportViewModel> salesReport, List<ExpensesReportViewModel> miscellaneousExpenses)
        {
            sheet.Cell(2, 2).Value =
                $"Resumen de ingresos/Egresos desde {inventoryReport.From!.Value:dd/MM/yyyy} hasta {inventoryReport.To!.Value:dd/MM/yyyy}";
            sheet.Range(2, 2, 2, 3).Merge();

            WriteRow(sheet, 3, "Productos ingresados", inventoryReport.inventoryInEntriesDetails.Sum(p => p.Quantity).ToString("0.##"));
            WriteCurrencyRow(sheet, 4, "Costo total de productos ingresados", inCost, XLColor.Red);

            WriteRow(sheet, 5, "Productos retirados", inventoryReport.inventoryOutEntriesDetails.Sum(p => p.Quantity).ToString("0.##"));
            WriteCurrencyRow(sheet, 6, "Costo total de productos retirados", outCost, XLColor.Red);

            WriteRow(sheet, 7, "Gastos miscelaneos", miscellaneousExpenses.Count.ToString("0.##"));
            WriteCurrencyRow(sheet, 8, "Costo total de miscelaneos", miscTotal, XLColor.Red);

            WriteRow(sheet, 9, "Productos vendidos", salesReport.Sum(p => p.ProductsQuantity).ToString("0.##"));
            WriteCurrencyRow(sheet, 10, "Total de ingresos por ventas", salesTotal, XLColor.Green);

            // Ganancia neta
            var netCell = WriteCurrencyRow(sheet, 11, "Ganancia neta (ventas - (gastos + costo de productos))", netProfit, netProfit <= 0 ? XLColor.Red : XLColor.Green);
            netCell.Style.Font.FontSize = 14;
        }

        private static void BuildResumenProductosSection(IXLWorksheet sheet, List<ProductsReportViewModel> productsReport)
        {
            sheet.Cell(2, 6).Value = "Productos";
            sheet.Range(2, 6, 2, 7).Merge();

            WriteRow(sheet, 3, "Cantidad de productos registrados", productsReport.Count.ToString("0.##"), 6, 7);
            WriteCurrencyRow(sheet, 4, "Inversión total", productsReport.Sum(p => p.TotalCost), XLColor.Green, 6, 7);
        }

        private static void ApplyGeneralFormatting(IXLWorksheet sheet)
        {
            sheet.Columns(1, 1).Width = 5;
            sheet.Columns(4, 5).Width = 5;
            sheet.Column(2).Width = 60;
            sheet.Column(3).Width = 30;
            sheet.Column(6).Width = 60;
            sheet.Column(7).Width = 30;

            sheet.Row(2).CellsUsed().Style.Fill.BackgroundColor = XLColor.AliceBlue;
            sheet.Row(2).Style.Font.Bold = true;
            sheet.Row(2).Style.Font.Shadow = true;
            sheet.Row(2).Style.Font.FontSize = 16;

            sheet.Range(2, 2, 11, 3).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            sheet.Range(2, 2, 11, 3).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            sheet.Range(2, 6, 4, 7).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            sheet.Range(2, 6, 4, 7).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);

            for (int i = 3; i <= 11; i++)
            {
                ApplyRowHeaderStyle(sheet, i, 2);
                if (i < 5) ApplyRowHeaderStyle(sheet, i, 6);
            }

            sheet.Range(3, 2, 11, 3).Style.Font.FontSize = 13;
            sheet.Range(3, 6, 4, 7).Style.Font.FontSize = 13;
        }


        private static void WriteRow(IXLWorksheet sheet, int row, string label, string value, int colLabel = 2, int colValue = 3)
        {
            sheet.Cell(row, colLabel).Value = label;
            sheet.Cell(row, colValue).Value = value;
        }

        private static IXLCell WriteCurrencyRow(IXLWorksheet sheet, int row, string label, decimal amount, XLColor fontColor, int colLabel = 2, int colValue = 3)
        {
            WriteRow(sheet, row, label, amount.ToString("C2", UsCulture), colLabel, colValue);

            var cell = sheet.Cell(row, colValue);
            cell.Style.NumberFormat.Format = CurrencyFormat;
            cell.Style.Font.Bold = true;
            cell.Style.Font.FontColor = fontColor;

            return cell;
        }

        private static void ApplyRowHeaderStyle(IXLWorksheet sheet, int row, int col)
        {
            var cell = sheet.Cell(row, col);
            cell.Style.Font.Bold = true;
            cell.Style.Font.Shadow = true;
            cell.Style.Fill.BackgroundColor = XLColor.AliceBlue;
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
