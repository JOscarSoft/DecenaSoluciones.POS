using ClosedXML.Excel;
using DecenaSoluciones.POS.Shared.Dtos;
using System.Data;
using System.Globalization;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    internal class InventoryExcelReports
    {

        public static void GetInventoryResumeWorkSheet(InventoryReportViewModel report, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet("Resumen de inventario");


            decimal inCost = report.inventoryInEntriesDetails.Sum(p => p.TotalCost);
            decimal outCost = report.inventoryOutEntriesDetails.Sum(p => p.TotalCost);
            sheet1.Cell(1 ,1).Value = $"Resumen de inventario desde {report.From!.Value:dd/MM/yyyy} hasta {report.To!.Value:dd/MM/yyyy}";
            sheet1.Cell(2, 1).Value = "Total de entradas";
            sheet1.Cell(3, 1).Value = "Productos ingresados";
            sheet1.Cell(4, 1).Value = "Costo total de productos ingresados";
            sheet1.Cell(5, 1).Value = "Total de salidas";
            sheet1.Cell(6, 1).Value = "Productos retirados";
            sheet1.Cell(7, 1).Value = "Costo total de productos retirados";
            sheet1.Cell(8, 1).Value = "Diferencia entradas/salidas";
            sheet1.Cell(2, 2).Value = report.inventoryInEntries.Count.ToString();
            sheet1.Cell(3, 2).Value = report.inventoryInEntriesDetails.Sum(p => p.Quantity).ToString();
            sheet1.Cell(4, 2).Value = inCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(5, 2).Value = report.inventoryOutEntries.Count.ToString();
            sheet1.Cell(6, 2).Value = report.inventoryOutEntriesDetails.Sum(p => p.Quantity).ToString();
            sheet1.Cell(7, 2).Value = outCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
            sheet1.Cell(8, 2).Value = (inCost - outCost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));


            sheet1.Range(sheet1.Cell(1, 1), sheet1.Cell(1, 2)).Merge();
            sheet1.Range(sheet1.Cell(2, 1), sheet1.Cell(8, 2)).Style.Font.FontSize = 13;
            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.AliceBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;
            sheet1.Row(1).Style.Font.FontSize = 16;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 1).Style.Font.Bold = true;
                sheet1.Cell(i, 1).Style.Font.Shadow = true;
                sheet1.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.AliceBlue;
            }
            sheet1.Cell(3, 2).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(6, 2).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(7, 2).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            sheet1.Cell(3, 2).Style.Font.Bold = true;
            sheet1.Cell(6, 2).Style.Font.Bold = true;
            sheet1.Cell(7, 2).Style.Font.Bold = true;

            sheet1.Columns(1, 2).Width = 45;
        }

        public static void GetInventoryInWorkSheet(List<InventoryInReport> inventoryInEntries, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetInventoryInReportDataTable(inventoryInEntries), "Entradas de inventario");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(inventoryInEntries.Count() + 2).Cell(3).Value = "Totales";
            sheet1.Row(inventoryInEntries.Count() + 2).Cell(4).Value = inventoryInEntries.Sum(p => p.ProductQuantity);
            sheet1.Row(inventoryInEntries.Count() + 2).Cell(5).Value = inventoryInEntries.Sum(p => p.TotalCost);
            sheet1.Row(inventoryInEntries.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(inventoryInEntries.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 5).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }

        public static void GetInventoryOutWorkSheet(List<InventoryOutReport> inventoryOutEntries, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetInventoryOutReportDataTable(inventoryOutEntries), "Salidas de inventario");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(inventoryOutEntries.Count() + 2).Cell(2).Value = "Totales";
            sheet1.Row(inventoryOutEntries.Count() + 2).Cell(3).Value = inventoryOutEntries.Sum(p => p.ProductQuantity);
            sheet1.Row(inventoryOutEntries.Count() + 2).Cell(4).Value = inventoryOutEntries.Sum(p => p.TotalCost);
            sheet1.Row(inventoryOutEntries.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(inventoryOutEntries.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 4).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }

        public static void GetInventoryInDetailedWorkSheet(List<InventoryInDetailReport> inventoryInEntryDetails, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetInventoryInDetailedReportDataTable(inventoryInEntryDetails), "Detalle de entradas");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(inventoryInEntryDetails.Count() + 2).Cell(3).Value = "Totales";
            sheet1.Row(inventoryInEntryDetails.Count() + 2).Cell(4).Value = inventoryInEntryDetails.Sum(p => p.UnitPrice);
            sheet1.Row(inventoryInEntryDetails.Count() + 2).Cell(5).Value = inventoryInEntryDetails.Sum(p => p.UnitCost);
            sheet1.Row(inventoryInEntryDetails.Count() + 2).Cell(6).Value = inventoryInEntryDetails.Sum(p => p.Quantity);
            sheet1.Row(inventoryInEntryDetails.Count() + 2).Cell(7).Value = inventoryInEntryDetails.Sum(p => p.TotalCost);
            sheet1.Row(inventoryInEntryDetails.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(inventoryInEntryDetails.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 4).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 5).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 7).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }

        public static void GetInventoryOutDetailedWorkSheet(List<InventoryOutDetailReport> inventoryOutEntryDetails, XLWorkbook wb)
        {
            var sheet1 = wb.AddWorksheet(GetInventoryOutDetailedReportDataTable(inventoryOutEntryDetails), "Detalle de salidas");

            sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
            sheet1.Row(1).Style.Font.Bold = true;
            sheet1.Row(1).Style.Font.Shadow = true;

            sheet1.Row(inventoryOutEntryDetails.Count() + 2).Cell(2).Value = "Totales";
            sheet1.Row(inventoryOutEntryDetails.Count() + 2).Cell(3).Value = inventoryOutEntryDetails.Sum(p => p.UnitPrice);
            sheet1.Row(inventoryOutEntryDetails.Count() + 2).Cell(4).Value = inventoryOutEntryDetails.Sum(p => p.UnitCost);
            sheet1.Row(inventoryOutEntryDetails.Count() + 2).Cell(5).Value = inventoryOutEntryDetails.Sum(p => p.Quantity);
            sheet1.Row(inventoryOutEntryDetails.Count() + 2).Cell(7).Value = inventoryOutEntryDetails.Sum(p => p.TotalCost);
            sheet1.Row(inventoryOutEntryDetails.Count() + 2).CellsUsed().Style.Font.Bold = true;
            sheet1.Row(inventoryOutEntryDetails.Count() + 2).CellsUsed().Style.Font.Shadow = true;

            for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
            {
                sheet1.Cell(i, 3).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 4).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                sheet1.Cell(i, 7).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
            }

            sheet1.Columns().AdjustToContents();
        }

        private static DataTable GetInventoryResumeReportDataTable(InventoryReportViewModel reportViewModel)
        {
            var dt = new DataTable();
            dt.TableName = "InventoryResumeReport";
            // dt.Columns.Add($"Resumen de inventario desde {reportViewModel.From!.Value:dd/MM/yyyy} hasta {reportViewModel.To!.Value:dd/MM/yyyy}", typeof(string));
            dt.Columns.Add("", typeof(string));
            dt.Columns.Add("", typeof(string));

            decimal inCost = reportViewModel.inventoryInEntriesDetails.Sum(p => p.TotalCost);
            decimal outCost = reportViewModel.inventoryOutEntriesDetails.Sum(p => p.TotalCost);
            dt.Rows.Add("Total de entradas", reportViewModel.inventoryInEntries.Count.ToString());
            dt.Rows.Add("Productos ingresados", reportViewModel.inventoryInEntriesDetails.Sum(p => p.Quantity).ToString());
            dt.Rows.Add("Costo total de productos ingresados", inCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")));
            dt.Rows.Add("Total de salidas", reportViewModel.inventoryOutEntries.Count.ToString());
            dt.Rows.Add("Productos retirados", reportViewModel.inventoryOutEntriesDetails.Sum(p => p.Quantity).ToString());
            dt.Rows.Add("Costo total de productos retirados", outCost.ToString("C2", CultureInfo.CreateSpecificCulture("en-US")));
            dt.Rows.Add("Diferencia entradas/salidas", (inCost - outCost).ToString("C2", CultureInfo.CreateSpecificCulture("en-US")));

            return dt;
        }

        private static DataTable GetInventoryInReportDataTable(List<InventoryInReport> inventoryInEntries)
        {
            var dt = new DataTable();
            dt.TableName = "InventoryInReport";
            dt.Columns.Add("Proveedor", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Cantidad de productos", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            if (inventoryInEntries.Count > 0)
            {
                inventoryInEntries.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.ProviderName, item.CreationDate, item.UserName, item.ProductQuantity, item.TotalCost);
                });
            }

            return dt;
        }

        private static DataTable GetInventoryOutReportDataTable(List<InventoryOutReport> inventoryOutEntries)
        {
            var dt = new DataTable();
            dt.TableName = "InventoryOutReport";
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Cantidad de productos", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            if (inventoryOutEntries.Count > 0)
            {
                inventoryOutEntries.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
                {
                    dt.Rows.Add( item.CreationDate, item.UserName, item.ProductQuantity, item.TotalCost);
                });
            }

            return dt;
        }

        private static DataTable GetInventoryInDetailedReportDataTable(List<InventoryInDetailReport> inventoryInEntryDetails)
        {
            var dt = new DataTable();
            dt.TableName = "InventoryInDetailedReport";
            dt.Columns.Add("Proveedor", typeof(string));
            dt.Columns.Add("Código Producto", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Costo", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            if (inventoryInEntryDetails.Count > 0)
            {
                inventoryInEntryDetails.OrderBy(p => p.ProviderName).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.ProviderName, item.ProductCode, item.ProductDescription, item.UnitPrice, item.UnitCost, item.Quantity, item.TotalCost);
                });
            }

            return dt;
        }

        private static DataTable GetInventoryOutDetailedReportDataTable(List<InventoryOutDetailReport> inventoryOutEntryDetails)
        {
            var dt = new DataTable();
            dt.TableName = "InventoryOutDetailedReport";
            dt.Columns.Add("Código Producto", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Costo", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));
            dt.Columns.Add("Comentario", typeof(string));

            if (inventoryOutEntryDetails.Count > 0)
            {
                inventoryOutEntryDetails.OrderBy(p => p.ProductCode).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.ProductCode, item.ProductDescription, item.UnitPrice, item.UnitCost, item.Quantity, item.TotalCost, item.Comments);
                });
            }

            return dt;
        }
    }
}
