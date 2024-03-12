using DecenaSoluciones.POS.Shared.Dtos;

namespace DecenaSoluciones.POS.API.Services
{
    public interface IReportService
    {
        Task<DashboardViewModel> GetDashboardReport();
        Task<List<InventoryReportViewModel>> GetInventoryReport();
        Task<List<SalesReportViewModel>> GetSalesReport(DateOnly fromDate, DateOnly toDate);
        Task<List<SoldProductsReportViewModel>> GetSoldProductsReport(DateOnly fromDate, DateOnly toDate);
    }
}
