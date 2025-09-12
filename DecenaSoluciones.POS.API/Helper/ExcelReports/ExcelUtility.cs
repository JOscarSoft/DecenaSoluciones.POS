using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class ExcelUtility
    {
        public static XLWorkbook GenerateSoldProductExcelReport(List<SoldProductsReportViewModel> report)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GenerateSoldProductExcelWorksheet(report, wb);
            return wb;
        }

        public static XLWorkbook GenerateSalesExcelReport(List<SalesReportViewModel> report)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GenerateSalesReportWorksheet(report, wb);
            return wb;
        }

        public static XLWorkbook GenerateInventoryExcelReport(InventoryReportViewModel report)
        {
            var wb = new XLWorkbook();
            InventoryExcelReports.GenerateInventorySummaryWorksheet(report, wb);
            InventoryExcelReports.GenerateInventoryInWorksheet(report.inventoryInEntries, wb);
            InventoryExcelReports.GenerateInventoryInDetailWorksheet(report.inventoryInEntriesDetails, wb);
            InventoryExcelReports.GenerateInventoryOutWorksheet(report.inventoryOutEntries, wb);
            InventoryExcelReports.GenerateInventoryOutDetailWorksheet(report.inventoryOutEntriesDetails, wb);
            return wb;
        }

        public static XLWorkbook GenerateExpenseAndIncomeExcelReport(
            List<SalesReportViewModel> salesReport,
            InventoryReportViewModel inventoryReport,
            List<ExpensesReportViewModel> miscellaneousExpenses,
            List<ProductsReportViewModel> productsReport)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GenerateSummaryWorksheet(inventoryReport, salesReport, miscellaneousExpenses, productsReport, wb);
            SalesExcelReports.GenerateSalesReportWorksheet(salesReport, wb);
            InventoryExcelReports.GenerateInventoryInWorksheet(inventoryReport.inventoryInEntries, wb);
            InventoryExcelReports.GenerateInventoryOutWorksheet(inventoryReport.inventoryOutEntries, wb);
            ExpensesExcelReports.GenerateExpensesWorksheet(miscellaneousExpenses, wb);
            return wb;
        }

        public static XLWorkbook GenerateProductsExcelReport(List<ProductsReportViewModel> result)
        {
            var wb = new XLWorkbook();
            ProductsExcelReports.GenerateProductsExcelWorksheet(result, wb);
            return wb;
        }
    }
}