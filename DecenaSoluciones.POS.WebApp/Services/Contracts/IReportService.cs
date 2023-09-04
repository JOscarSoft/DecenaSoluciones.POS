using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.WebApp.Services
{
    public interface IReportService
    {
        Task<ApiResponse<DashboardViewModel>> GetDashboardReport();
        Task<Stream> GenerateReport(DateOnly fromDate, DateOnly toDate, EnumReportType type);
    }
}
