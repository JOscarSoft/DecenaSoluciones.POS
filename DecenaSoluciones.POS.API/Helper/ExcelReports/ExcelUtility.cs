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
            List<ExpensesReportViewModel> miscellaneousExpenses,
            List<ProductsReportViewModel> productsReport)
        {
            var wb = new XLWorkbook();
            SalesExcelReports.GetSalesAndInventoryResumeWorkSheet(inventoryReport, salesReport, miscellaneousExpenses, productsReport, wb);
            SalesExcelReports.GetSalesReportWorkSheet(salesReport, wb);
            InventoryExcelReports.GetInventoryInWorkSheet(inventoryReport.inventoryInEntries, wb);
            InventoryExcelReports.GetInventoryOutWorkSheet(inventoryReport.inventoryOutEntries, wb);
            ExpensesExcelReports.GetExpensesWorkSheet(miscellaneousExpenses, wb);
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
