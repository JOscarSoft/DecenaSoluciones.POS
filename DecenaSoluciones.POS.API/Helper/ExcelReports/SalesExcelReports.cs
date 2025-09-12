using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class SalesExcelReports
    {
        public static void GenerateSoldProductExcelWorksheet(List<SoldProductsReportViewModel> report, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateSoldProductsReportDataTable(report);
            var sheet = wb.Worksheets.Add(dataTable, "Ventas por productos");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = report.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 2, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 3, report.Sum(p => p.Quantity));
            ReportHelper.WriteCell(sheet, totalsRow, 4, report.Sum(p => p.SalesTotal));
            ReportHelper.WriteCell(sheet, totalsRow, 5, report.Sum(p => p.Stock));

            ReportHelper.FormatColumnAsCurrency(sheet, 4);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }

        public static void GenerateSalesReportWorksheet(List<SalesReportViewModel> report, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateSalesReportDataTable(report);
            var sheet = wb.Worksheets.Add(dataTable, "Reporte de ventas");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = report.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 4, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 5, report.Sum(p => p.ProductsQuantity));
            ReportHelper.WriteCell(sheet, totalsRow, 6, report.Sum(p => p.Total));
            ReportHelper.WriteCell(sheet, totalsRow, 7, report.Sum(p => p.PayedAmount));
            ReportHelper.WriteCell(sheet, totalsRow, 8, report.Sum(p => p.Devolution));

            ReportHelper.FormatColumnAsCurrency(sheet, 6);
            ReportHelper.FormatColumnAsCurrency(sheet, 7);
            ReportHelper.FormatColumnAsCurrency(sheet, 8);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }

        public static void GenerateSummaryWorksheet(
            InventoryReportViewModel inventoryReport,
            List<SalesReportViewModel> salesReport,
            List<ExpensesReportViewModel> miscellaneousExpenses,
            List<ProductsReportViewModel> productsReport,
            XLWorkbook workbook)
        {
            var sheet = workbook.AddWorksheet("Resumen");

            BuildIncomeAndExpensesSection(sheet, inventoryReport, salesReport, miscellaneousExpenses);
            BuildProductsSection(sheet, productsReport);

            ApplyGeneralFormatting(sheet);
        }

        private static void BuildIncomeAndExpensesSection(
            IXLWorksheet sheet,
            InventoryReportViewModel inventoryReport,
            List<SalesReportViewModel> salesReport,
            List<ExpensesReportViewModel> miscellaneousExpenses)
        {
            decimal totalInventoryIn = inventoryReport.inventoryInEntriesDetails.Sum(p => p.TotalCost);
            decimal totalInventoryOut = inventoryReport.inventoryOutEntriesDetails.Sum(p => p.TotalCost);
            decimal totalSales = salesReport.Sum(p => p.Total!.Value);
            decimal totalMiscExpenses = miscellaneousExpenses.Sum(p => p.TotalCost);
            decimal netProfit = totalSales - (totalInventoryIn + totalInventoryOut + totalMiscExpenses);

            sheet.Cell(2, 2).Value = $"Resumen de ingresos/Egresos desde {inventoryReport.From!.Value:dd/MM/yyyy} hasta {inventoryReport.To!.Value:dd/MM/yyyy}";
            sheet.Range(2, 2, 2, 3).Merge();

            ReportHelper.WriteLabeledRow(sheet, 3, 2, "Productos ingresados", 3, inventoryReport.inventoryInEntriesDetails.Sum(p => p.Quantity));
            ReportHelper.WriteCurrencyRow(sheet, 4, 2, "Costo total de productos ingresados", 3, totalInventoryIn, XLColor.Red);

            ReportHelper.WriteLabeledRow(sheet, 5, 2, "Productos retirados", 3, inventoryReport.inventoryOutEntriesDetails.Sum(p => p.Quantity));
            ReportHelper.WriteCurrencyRow(sheet, 6, 2, "Costo total de productos retirados", 3, totalInventoryOut, XLColor.Red);

            ReportHelper.WriteLabeledRow(sheet, 7, 2, "Gastos miscelaneos", 3, miscellaneousExpenses.Count);
            ReportHelper.WriteCurrencyRow(sheet, 8, 2, "Costo total de miscelaneos", 3, totalMiscExpenses, XLColor.Red);

            ReportHelper.WriteLabeledRow(sheet, 9, 2, "Productos vendidos", 3, salesReport.Sum(p => p.ProductsQuantity));
            ReportHelper.WriteCurrencyRow(sheet, 10, 2, "Total de ingresos por ventas", 3, totalSales, XLColor.Green);

            var netCell = ReportHelper.WriteCurrencyRow(sheet, 11, 2, "Ganancia neta (ventas - (gastos + costo de productos))", 3, netProfit, netProfit <= 0 ? XLColor.Red : XLColor.Green);
            netCell.Style.Font.FontSize = 14;
            sheet.Cell(11, 2).Style.Font.FontSize = 14;
        }

        private static void BuildProductsSection(IXLWorksheet sheet, List<ProductsReportViewModel> productsReport)
        {
            sheet.Cell(2, 6).Value = "Productos";
            sheet.Range(2, 6, 2, 7).Merge();

            ReportHelper.WriteLabeledRow(sheet, 3, 6, "Cantidad de productos registrados", 7, productsReport.Count);
            ReportHelper.WriteCurrencyRow(sheet, 4, 6, "Inversión total", 7, productsReport.Sum(p => p.TotalCost), XLColor.Green);
        }

        private static void ApplyGeneralFormatting(IXLWorksheet sheet)
        {
            sheet.Column(1).Width = 5;
            sheet.Column(2).Width = 60;
            sheet.Column(3).Width = 30;
            sheet.Column(4).Width = 5;
            sheet.Column(5).Width = 5;
            sheet.Column(6).Width = 60;
            sheet.Column(7).Width = 30;

            var titleHeader = sheet.Row(2).CellsUsed();
            titleHeader.Style.Fill.BackgroundColor = XLColor.AliceBlue;
            titleHeader.Style.Font.Bold = true;
            titleHeader.Style.Font.Shadow = true;
            titleHeader.Style.Font.FontSize = 16;

            var incomeRange = sheet.Range(2, 2, 11, 3);
            incomeRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            incomeRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            incomeRange.Style.Font.FontSize = 13;

            var productsRange = sheet.Range(2, 6, 4, 7);
            productsRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            productsRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            productsRange.Style.Font.FontSize = 13;

            for (int i = 3; i <= 11; i++)
            {
                var cell = sheet.Cell(i, 2);
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.AliceBlue;

                if (i <= 4)
                {
                    var productCell = sheet.Cell(i, 6);
                    productCell.Style.Font.Bold = true;
                    productCell.Style.Fill.BackgroundColor = XLColor.AliceBlue;
                }
            }
        }
    }
}