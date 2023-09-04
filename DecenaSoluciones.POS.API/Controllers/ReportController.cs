using ClosedXML.Excel;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.API.Services;
using DecenaSoluciones.POS.Shared.Dtos;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace DecenaSoluciones.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Route("Dashboard")]
        public async Task<IActionResult> GetDashboardReport()
        {
            var apiResponse = new ApiResponse<DashboardViewModel>();
            try
            {
                apiResponse.Result = await _reportService.GetDashboardReport();
                apiResponse.Success = true;
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
            }

            return Ok(apiResponse);
        }

        [HttpGet]
        [Route("GetSalesReport/{fromDate}/{toDate}")]
        public async Task<IActionResult> GetSalesReport(string fromDate, string toDate)
        {
            var fromDateFormated = DateOnly.ParseExact(fromDate, "dd-MM-yyyy");
            var toDateFormated = DateOnly.ParseExact(toDate, "dd-MM-yyyy");
            var result = await _reportService.GetSalesReport(fromDateFormated, toDateFormated);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet1 = wb.AddWorksheet(GetSalesReportDataTable(result), "Reporte de ventas");

                sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;
                sheet1.Row(1).Style.Font.Bold = true;
                sheet1.Row(1).Style.Font.Shadow = true;

                sheet1.Row(result.Count() + 2).Cell(4).Value = "Totales";
                sheet1.Row(result.Count() + 2).Cell(5).Value = result.Sum(p => p.ProductsQuantity);
                sheet1.Row(result.Count() + 2).Cell(6).Value = result.Sum(p => p.Total);
                sheet1.Row(result.Count() + 2).CellsUsed().Style.Font.Bold = true;
                sheet1.Row(result.Count() + 2).CellsUsed().Style.Font.Shadow = true;

                for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
                {
                    sheet1.Cell(i, 6).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                }

                sheet1.Columns().AdjustToContents();
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de ventas.xlsx");
                }
            }
        }

        [HttpGet]
        [Route("GetSoldProductsReport/{fromDate}/{toDate}")]
        public async Task<IActionResult> GetSoldProductsReport(string fromDate, string toDate)
        {
            var fromDateFormated = DateOnly.ParseExact(fromDate, "dd-MM-yyyy");
            var toDateFormated = DateOnly.ParseExact(toDate, "dd-MM-yyyy");
            var result = await _reportService.GetSoldProductsReport(fromDateFormated, toDateFormated);

            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet1 = wb.AddWorksheet(GetSoldProductsReportDataTable(result), "Ventas por productos");

                sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.DarkBlue;

                sheet1.Row(1).Style.Font.Bold = true;
                sheet1.Row(1).Style.Font.Shadow = true;

                sheet1.Row(result.Count() + 2).Cell(2).Value = "Totales";
                sheet1.Row(result.Count() + 2).Cell(3).Value = result.Sum(p => p.Quantity);
                sheet1.Row(result.Count() + 2).Cell(4).Value = result.Sum(p => p.SalesTotal);
                sheet1.Row(result.Count() + 2).Cell(5).Value = result.Sum(p => p.Stock);
                sheet1.Row(result.Count() + 2).CellsUsed().Style.Font.Bold = true;
                sheet1.Row(result.Count() + 2).CellsUsed().Style.Font.Shadow = true;

                for (int i = 2; i <= sheet1.RowsUsed().Count(); i++)
                {
                    sheet1.Cell(i, 4).Style.NumberFormat.Format = @"[$$-en-US]#,##0.00_);[Red]([$$-en-US]#,##0.00)";
                }

                sheet1.Columns().AdjustToContents();
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ventas por productos.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetSalesReportDataTable(List<SalesReportViewModel> sales)
        {
            DataTable dt = new DataTable();
            dt.TableName = "SalesReport";
            dt.Columns.Add("Código", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Fecha", typeof(DateOnly));
            dt.Columns.Add("Usuario", typeof(string));
            dt.Columns.Add("Productos", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));

            if (sales.Count > 0)
            {
                sales.OrderBy(p => p.CreationDate).ToList().ForEach(item =>
                {
                    dt.Rows.Add(item.Code, item.CustomerName, DateOnly.FromDateTime(item.CreationDate), item.UserName, item.ProductsQuantity, item.Total);
                });
            }

            return dt;
        }

        [NonAction]
        private DataTable GetSoldProductsReportDataTable(List<SoldProductsReportViewModel> sales)
        {
            DataTable dt = new DataTable();
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
    }
}
