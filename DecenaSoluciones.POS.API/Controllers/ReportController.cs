using DecenaSoluciones.POS.API.Helper;
using DecenaSoluciones.POS.API.Services;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            var excelReport = ExcelUtility.GenerateSalesExcelReport(result);

            using var ms = new MemoryStream();
            excelReport.SaveAs(ms);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de ventas.xlsx");
        }

        [HttpGet]
        [Route("GetSoldProductsReport/{fromDate}/{toDate}")]
        public async Task<IActionResult> GetSoldProductsReport(string fromDate, string toDate)
        {
            var fromDateFormated = DateOnly.ParseExact(fromDate, "dd-MM-yyyy");
            var toDateFormated = DateOnly.ParseExact(toDate, "dd-MM-yyyy");
            var result = await _reportService.GetSoldProductsReport(fromDateFormated, toDateFormated);
            
            var excelReport = ExcelUtility.GenerateSoldProductExcelReport(result);

            using var ms = new MemoryStream();
            excelReport.SaveAs(ms);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Ventas por productos.xlsx");
        }

        [HttpGet]
        [Route("GetProductsReport")]
        public async Task<IActionResult> GetProductsReport()
        {
            var result = await _reportService.GetProductsReport();

            var excelReport = ExcelUtility.GenerateProductsExcelReport(result);

            using var ms = new MemoryStream();
            excelReport.SaveAs(ms);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Reporte de inventario.xlsx");
        }
    }
}
