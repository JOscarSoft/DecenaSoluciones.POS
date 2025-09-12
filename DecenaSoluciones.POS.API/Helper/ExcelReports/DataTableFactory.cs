using DecenaSoluciones.POS.Shared.Dtos;
using System.Data;

namespace DecenaSoluciones.POS.API.Helper.ExcelReports
{
    /// <summary>
    /// Factory class for creating DataTable objects from report view models.
    /// </summary>
    internal static class DataTableFactory
    {
        public static DataTable CreateProductsReportDataTable(List<ProductsReportViewModel> products)
        {
            var dt = new DataTable("ProductsReport");
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Precio Compra", typeof(decimal));
            dt.Columns.Add("Precio Venta", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("Total Compra", typeof(decimal));
            dt.Columns.Add("Total Venta", typeof(decimal));
            dt.Columns.Add("Ganancias", typeof(decimal));

            products.OrderBy(p => p.Description).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.Code, item.Description, item.Cost, item.Price, item.stock, item.TotalCost, item.TotalPrice, item.Revenue);
            });

            return dt;
        }

        public static DataTable CreateInventoryInReportDataTable(List<InventoryInReport> entries)
        {
            var dt = new DataTable("InventoryInReport");
            dt.Columns.Add("Proveedor", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Cantidad de productos", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            entries.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.ProviderName, item.CreationDate, item.UserName, item.ProductQuantity, item.TotalCost);
            });

            return dt;
        }

        public static DataTable CreateInventoryOutReportDataTable(List<InventoryOutReport> entries)
        {
            var dt = new DataTable("InventoryOutReport");
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Cantidad de productos", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            entries.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.CreationDate, item.UserName, item.ProductQuantity, item.TotalCost);
            });

            return dt;
        }

        public static DataTable CreateInventoryInDetailReportDataTable(List<InventoryInDetailReport> details)
        {
            var dt = new DataTable("InventoryInDetailedReport");
            dt.Columns.Add("Proveedor", typeof(string));
            dt.Columns.Add("Código Producto", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Costo", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            details.OrderBy(p => p.ProviderName).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.ProviderName, item.ProductCode, item.ProductDescription, item.UnitPrice, item.UnitCost, item.Quantity, item.TotalCost);
            });

            return dt;
        }

        public static DataTable CreateInventoryOutDetailReportDataTable(List<InventoryOutDetailReport> details)
        {
            var dt = new DataTable("InventoryOutDetailedReport");
            dt.Columns.Add("Código Producto", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Comentario", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Costo", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(decimal));
            dt.Columns.Add("Costo Total", typeof(decimal));

            details.OrderBy(p => p.ProductCode).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.ProductCode, item.ProductDescription, item.Comments, item.UnitPrice, item.UnitCost, item.Quantity, item.TotalCost);
            });

            return dt;
        }

        public static DataTable CreateSoldProductsReportDataTable(List<SoldProductsReportViewModel> sales)
        {
            var dt = new DataTable("SoldProductsReport");
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Stock", typeof(int));

            sales.OrderBy(p => p.Description).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.Code, item.Description, item.Quantity, item.SalesTotal, item.Stock);
            });

            return dt;
        }

        public static DataTable CreateSalesReportDataTable(List<SalesReportViewModel> sales)
        {
            var dt = new DataTable("SalesReport");
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateOnly));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Productos", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("Monto pagado", typeof(decimal));
            dt.Columns.Add("Devuelta", typeof(decimal));

            sales.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.Code, item.CustomerName, DateOnly.FromDateTime(item.CreationDate), item.UserName, item.ProductsQuantity, item.Total, item.PayedAmount, item.Devolution);
            });

            return dt;
        }

        public static DataTable CreateExpensesReportDataTable(List<ExpensesReportViewModel> expenses)
        {
            var dt = new DataTable("MiscellaneousExpensesReport");
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateOnly));
            dt.Columns.Add("Descripción", typeof(string));
            dt.Columns.Add("Monto", typeof(decimal));

            expenses.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
            {
                dt.Rows.Add(item.UserName, DateOnly.FromDateTime(item.CreationDate), item.Comments, item.TotalCost);
            });

            return dt;
        }
    }
}