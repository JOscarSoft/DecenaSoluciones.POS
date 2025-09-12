using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class InventoryExcelReports
    {
        public static void GenerateInventorySummaryWorksheet(InventoryReportViewModel report, XLWorkbook wb)
        {
            var sheet = wb.AddWorksheet("Resumen de inventario");

            decimal inCost = report.inventoryInEntriesDetails.Sum(p => p.TotalCost);
            decimal outCost = report.inventoryOutEntriesDetails.Sum(p => p.TotalCost);

            // Title
            sheet.Cell(2, 2).Value = $"Resumen de inventario desde {report.From!.Value:dd/MM/yyyy} hasta {report.To!.Value:dd/MM/yyyy}";
            var titleRange = sheet.Range(sheet.Cell(2, 2), sheet.Cell(2, 3));
            titleRange.Merge();
            titleRange.Style.Font.Bold = true;
            titleRange.Style.Font.FontSize = 16;
            titleRange.Style.Fill.BackgroundColor = XLColor.AliceBlue;

            // Data Rows
            ReportHelper.WriteLabeledRow(sheet, 3, 2, "Total de entradas", 3, report.inventoryInEntries.Count);
            ReportHelper.WriteLabeledRow(sheet, 4, 2, "Productos ingresados", 3, report.inventoryInEntriesDetails.Sum(p => p.Quantity));
            ReportHelper.WriteCurrencyRow(sheet, 5, 2, "Costo total de productos ingresados", 3, inCost);

            ReportHelper.WriteLabeledRow(sheet, 6, 2, "Total de salidas", 3, report.inventoryOutEntries.Count);
            ReportHelper.WriteLabeledRow(sheet, 7, 2, "Productos retirados", 3, report.inventoryOutEntriesDetails.Sum(p => p.Quantity));
            ReportHelper.WriteCurrencyRow(sheet, 8, 2, "Costo total de productos retirados", 3, outCost);

            // Styling
            var dataRange = sheet.Range(sheet.Cell(2, 2), sheet.Cell(8, 3));
            dataRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            dataRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            dataRange.Style.Font.FontSize = 13;

            for (int i = 3; i <= 8; i++)
            {
                sheet.Cell(i, 2).Style.Font.Bold = true;
                sheet.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.AliceBlue;
            }

            sheet.Column(1).Width = 5;
            sheet.Column(2).Width = 50;
            sheet.Column(3).Width = 30;
        }

        public static void GenerateInventoryInWorksheet(List<InventoryInReport> entries, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateInventoryInReportDataTable(entries);
            var sheet = wb.Worksheets.Add(dataTable, "Entradas de inventario");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = entries.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 3, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 4, entries.Sum(p => p.ProductQuantity));
            ReportHelper.WriteCell(sheet, totalsRow, 5, entries.Sum(p => p.TotalCost));

            ReportHelper.FormatColumnAsCurrency(sheet, 5);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }

        public static void GenerateInventoryOutWorksheet(List<InventoryOutReport> entries, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateInventoryOutReportDataTable(entries);
            var sheet = wb.Worksheets.Add(dataTable, "Salidas de inventario");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = entries.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 2, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 3, entries.Sum(p => p.ProductQuantity));
            ReportHelper.WriteCell(sheet, totalsRow, 4, entries.Sum(p => p.TotalCost));

            ReportHelper.FormatColumnAsCurrency(sheet, 4);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }

        public static void GenerateInventoryInDetailWorksheet(List<InventoryInDetailReport> details, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateInventoryInDetailReportDataTable(details);
            var sheet = wb.Worksheets.Add(dataTable, "Detalle de entradas");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = details.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 3, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 4, details.Sum(p => p.UnitPrice));
            ReportHelper.WriteCell(sheet, totalsRow, 5, details.Sum(p => p.UnitCost));
            ReportHelper.WriteCell(sheet, totalsRow, 6, details.Sum(p => p.Quantity));
            ReportHelper.WriteCell(sheet, totalsRow, 7, details.Sum(p => p.TotalCost));

            ReportHelper.FormatColumnAsCurrency(sheet, 4);
            ReportHelper.FormatColumnAsCurrency(sheet, 5);
            ReportHelper.FormatColumnAsCurrency(sheet, 7);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }

        public static void GenerateInventoryOutDetailWorksheet(List<InventoryOutDetailReport> details, XLWorkbook wb)
        {
            var dataTable = DataTableFactory.CreateInventoryOutDetailReportDataTable(details);
            var sheet = wb.Worksheets.Add(dataTable, "Detalle de salidas");

            ReportHelper.ApplyHeaderStyle(sheet);

            var totalsRow = details.Count + 2;
            ReportHelper.WriteCell(sheet, totalsRow, 3, "Totales");
            ReportHelper.WriteCell(sheet, totalsRow, 4, details.Sum(p => p.UnitPrice));
            ReportHelper.WriteCell(sheet, totalsRow, 5, details.Sum(p => p.UnitCost));
            ReportHelper.WriteCell(sheet, totalsRow, 6, details.Sum(p => p.Quantity));
            ReportHelper.WriteCell(sheet, totalsRow, 7, details.Sum(p => p.TotalCost));

            ReportHelper.FormatColumnAsCurrency(sheet, 4);
            ReportHelper.FormatColumnAsCurrency(sheet, 5);
            ReportHelper.FormatColumnAsCurrency(sheet, 7);
            ReportHelper.ApplyTotalsRowStyle(sheet.Row(totalsRow));

            sheet.Columns().AdjustToContents();
        }
    }
}