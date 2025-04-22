using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;
using DocumentFormat.OpenXml.Bibliography;
using System.Data;

namespace DecenaSoluciones.POS.API.Helper
{
    public class ExcelUtility
    {
        public static XLWorkbook GenerateSoldProductExcelReport(List<SoldProductsReportViewModel> report)
        {
            var wb = new XLWorkbook();

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

            return wb;
        }

        public static XLWorkbook GenerateSalesExcelReport(List<SalesReportViewModel> report)
        {
            var wb = new XLWorkbook();

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

            return wb;
        }

        public static XLWorkbook GenerateInventoryExcelReport(List<InventoryReportViewModel> result)
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

        private static DataTable GetInventoryReportDataTable(List<InventoryReportViewModel> products)
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
