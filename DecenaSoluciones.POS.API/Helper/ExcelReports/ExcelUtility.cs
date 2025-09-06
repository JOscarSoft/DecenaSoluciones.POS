using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;
using DocumentFormat.OpenXml.Bibliography;
using System.Data;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class ExcelUtility
    {
        public static XLWorkbook GenerateSoldProductExcelReport(List<SoldProductsReportViewModel> report)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GetSoldProductExcelReport(report, wb);
            return wb;
        }

        public static XLWorkbook GenerateSalesExcelReport(List<SalesReportViewModel> report)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GetSalesReportWorkSheet(report, wb);
            return wb;
        }

        public static XLWorkbook GenerateInventoryExcelReport(InventoryReportViewModel report)
        {
            var wb = new XLWorkbook();
            InventoryExcelReports.GetInventoryResumeWorkSheet(report, wb);
            InventoryExcelReports.GetInventoryInWorkSheet(report.inventoryInEntries, wb);
            InventoryExcelReports.GetInventoryInDetailedWorkSheet(report.inventoryInEntriesDetails, wb);
            InventoryExcelReports.GetInventoryOutWorkSheet(report.inventoryOutEntries, wb);
            InventoryExcelReports.GetInventoryOutDetailedWorkSheet(report.inventoryOutEntriesDetails, wb);
            return wb;
        }

        public static XLWorkbook GenerateExpenseAndIncomeExcelReport(
            List<SalesReportViewModel> salesReport, 
            InventoryReportViewModel inventoryReport,
            List<ExpensesReportViewModel> miscellaneousExpenses)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GetSalesAndInventoryResumeWorkSheet(inventoryReport, salesReport, miscellaneousExpenses, wb);
            SalesExcelReports.GetSalesReportWorkSheet(salesReport, wb);
            InventoryExcelReports.GetInventoryInWorkSheet(inventoryReport.inventoryInEntries, wb);
            InventoryExcelReports.GetInventoryOutWorkSheet(inventoryReport.inventoryOutEntries, wb);
            ExpensesExcelReports.GetExpensesWorkSheet(miscellaneousExpenses, wb);
            return wb;
        }

        public static XLWorkbook GenerateProductsExcelReport(List<ProductsReportViewModel> result)
        {
            var wb = new XLWorkbook();

            var sheet1 = wb.AddWorksheet(GetInventoryReportDataTable(result), "Reporte de inventario");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(result.Count() + 2).Cell(4).Value = "Totales";
            sheet1.Row(result.Count() + 2).Cell(5).Value = result.Sum(p => p.stock);
            sheet1.Row(result.Count() + 2).Cell(6).Value = result.Sum(p => p.TotalCost);
            sheet1.Row(result.Count() + 2).Cell(7).Value = result.Sum(p => p.TotalPrice);
            sheet1.Row(result.Count() + 2).Cell(8).Value = result.Sum(p => p.Revenue);
            sheet1.Row(result.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(result.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 6).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 7).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 8).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();

            return wb;
        }

        private static DataTable GetInventoryReportDataTable(List<ProductsReportViewModel> products)
        {
            var dt = new DataTable();
            dt.TableName = "InventoryReport";
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Precio Compra", typeof(decimal));
            dt.Columns.Add("Precio Venta", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("Total Compra", typeof(decimal));
            dt.Columns.Add("Total Venta", typeof(decimal));
            dt.Columns.Add("Ganancias", typeof(decimal));

            if (products.Count > 0)
            {
                products.OrderBy(p => p.Description).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.Code, item.Description, item.Cost, item.Price, item.stock, item.TotalCost, item.TotalPrice, item.Revenue);
                });
            }

            return dt;
        }
    }
}
