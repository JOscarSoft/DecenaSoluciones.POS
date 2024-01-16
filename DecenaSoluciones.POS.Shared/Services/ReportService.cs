using DecenaSoluciones.POS.Shared.Dtos;
using System.Net.Http.Json;
using DecenaSoluciones.POS.Shared.Enums;

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

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStreamAsync();

                if (result == null)
                    throw new Exception("No se obtuvo respuesta del servicio de reportes.");

                return result;
            }
            else
                throw new Exception("Se produjo un error al procesar la petición.");
        }

        public async Task<ApiResponse<DashboardViewModel>> GetDashboardReport()
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResponse<DashboardViewModel>>("api/Report/Dashboard");

            if (result == null)
                throw new Exception("No se obtuvo respuesta del servicio de Reportes.");

            return result;
        }
    }
}
