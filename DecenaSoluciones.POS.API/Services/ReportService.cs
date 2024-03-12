using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace DecenaSoluciones.POS.API.Services
{
    public class ReportService : IReportService
    {
        private readonly DecenaSolucionesDBContext _dbContext;
        private readonly IMapper _mapper;

        public ReportService(DecenaSolucionesDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<DashboardViewModel> GetDashboardReport()
        {
            var result = new DashboardViewModel();
            var sales = await _dbContext.Sale
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .ToListAsync();

            result.SoldProductsPerWeek = sales.Where(p => p.CreationDate >= DateTime.Now.StartOfWeek(DayOfWeek.Monday) && p.CreationDate <= DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7))
                .Select(p => p.SaleProducts)
                .SelectMany(p => p)
                .Sum(p => p.Quantity);

            result.SoldProducts = sales.Select(p => p.SaleProducts).SelectMany(p => p).GroupBy(p => p.Product).Select(p => new SoldProductQuantityViewModel
            {
                ProductName = p.Key.Description,
                Quantity = p.Sum(p => p.Quantity)
            }).OrderByDescending(p => p.Quantity).Take(5).ToList();

            result.ExpiredMaintenances = (await _dbContext.CustomerProducts.Where(p => p.NextMaintenance.HasValue).ToListAsync())
                .Where(p => (p.NextMaintenance!.Value - DateTime.Now).TotalDays < 15)
                .Select(p => p.CustomerId)
                .Distinct().Count();
            
            result.ProductsWithEmptyStock = await _dbContext.Products.Where(p => p.stock <= 5).CountAsync();

            return result;
        }

        public async Task<List<InventoryReportViewModel>> GetInventoryReport()
        {
            var products = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<InventoryReportViewModel>>(products);
        }

        public async Task<List<SalesReportViewModel>> GetSalesReport(DateOnly fromDate, DateOnly toDate)
        {
            var sales = await _dbContext.Sale
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .Where(p => p.CreationDate >= fromDate.ToDateTime(TimeOnly.MinValue) && p.CreationDate <= toDate.ToDateTime(TimeOnly.MaxValue))
                .ToListAsync();

            return _mapper.Map<List<SalesReportViewModel>>(sales);
        }

        public async Task<List<SoldProductsReportViewModel>> GetSoldProductsReport(DateOnly fromDate, DateOnly toDate)
        {
            var sales = await _dbContext.Sale
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .Where(p => p.CreationDate >= fromDate.ToDateTime(TimeOnly.MinValue) && p.CreationDate <= toDate.ToDateTime(TimeOnly.MaxValue))
                .ToListAsync();

            var saleProducts = sales.Select(p => p.SaleProducts)
                .SelectMany(p => p)
                .GroupBy(p => p.Product)
                .Select(p => new SoldProductsReportViewModel
                {
                    Code = p.Key.Code,
                    Description = p.Key.Description,
                    Quantity = p.Sum(p => p.Quantity),
                    SalesTotal = p.Sum(p => p.Total),
                    Stock = p.Key.stock
                });

            return saleProducts.ToList();
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}
