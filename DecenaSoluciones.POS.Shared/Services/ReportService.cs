using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Enums;
using DecenaSoluciones.POS.Shared.Extensions;
using System.Net.Http.Json;

namespace DecenaSoluciones.POS.Shared.Services
{
    public class ReportService : IReportService
    {
        private readonly HttpClient _httpClient;
        public ReportService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
        }

        public async Task<Stream> GenerateReport(DateOnly fromDate, DateOnly toDate, EnumReportType reportType)
        {
            var reportUrl = reportType == EnumReportType.SoldProducts ? "api/Report/GetSoldProductsReport" : "api/Report/GetSalesReport";
            var response = await _httpClient.GetAsync($"{reportUrl}/{fromDate:dd-MM-yyyy}/{toDate:dd-MM-yyyy}");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadAsStreamAsync();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de reportes.");

            return result;
        }

        public async Task<Stream> GenerateProductsReport()
        {
            var response = await _httpClient.GetAsync("api/Report/GetProductsReport");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadAsStreamAsync();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de reportes.");

            return result;
        }

        public async Task<ApiResponse<DashboardViewModel>> GetDashboardReport()
        {
            var response = await _httpClient.GetAsync("api/Report/Dashboard");

            response.EnsureResponseStatus();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardViewModel>>();

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de reportes.");

            return result;
        }
    }
}
