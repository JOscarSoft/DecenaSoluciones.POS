using DecenaSoluciones.POS.Shared.Dtos;
using DecenaSoluciones.POS.Shared.Enums;

namespace DecenaSoluciones.POS.Shared.Services
{
    public interface IReportService
    {
        Task<ApiResponse<DashboardViewModel>> GetDashboardReport();
        Task<Stream> GenerateReport(DateOnly fromDate, DateOnly toDate, EnumReportType type);
    }
}
