using AutoMapper;
using DecenaSoluciones.POS.API.Models;
using DecenaSoluciones.POS.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                .Where(p => p.Dismissed == null || p.Dismissed == false)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .ToListAsync();

            result.SoldProductsPerWeek = sales.Where(p => p.CreationDate >= DateTime.Now.StartOfWeek(DayOfWeek.Monday) && p.CreationDate <= DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7))
                .Select(p => p.SaleProducts)
                .SelectMany(p => p)
                .Sum(p => (int)Math.Round(p.Quantity, MidpointRounding.AwayFromZero));

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

        public async Task<List<ProductsReportViewModel>> GetProductsReport()
        {
            var products = await _dbContext.Products.ToListAsync();
            return _mapper.Map<List<ProductsReportViewModel>>(products);
        }

        public async Task<InventoryReportViewModel> GetInventoryReport(DateOnly fromDate, DateOnly toDate)
        {
            var inventoryEntries = await _dbContext.InventoryEntries
                .Include(p => p.InventoryEntryType)
                .Include(p => p.Provider)
                .Include(p => p.InventoryEntryDetails)!
                .ThenInclude(p => p.Product)
                .Where(p => p.CreationDate >= fromDate.ToDateTime(TimeOnly.MinValue) && p.CreationDate <= toDate.ToDateTime(TimeOnly.MaxValue))
                .OrderByDescending(p => p.CreationDate)
                .ToListAsync();

            return _mapper.Map<InventoryReportViewModel>(inventoryEntries);
        }

        public async Task<List<SalesReportViewModel>> GetSalesReport(DateOnly fromDate, DateOnly toDate)
        {
            var sales = await _dbContext.Sale
                .Where(p => p.Dismissed == null || p.Dismissed == false)
                .Include(p => p.Customer)
                .Include(p => p.SaleProducts)!
                .ThenInclude(p => p.Product)
                .Where(p => p.CreationDate >= fromDate.ToDateTime(TimeOnly.MinValue) && p.CreationDate <= toDate.ToDateTime(TimeOnly.MaxValue))
                .ToListAsync();

            return _mapper.Map<List<SalesReportViewModel>>(sales);
        }

        public async Task<List<ExpensesReportViewModel>> GetMiscellaneousExpensesReport(DateOnly fromDate, DateOnly toDate)
        {
            var expenses = await _dbContext.MiscellaneousExpenses
                .Where(p => p.CreationDate >= fromDate.ToDateTime(TimeOnly.MinValue) && p.CreationDate <= toDate.ToDateTime(TimeOnly.MaxValue))
                .ToListAsync();
            return _mapper.Map<List<ExpensesReportViewModel>>(expenses);
        }

        public async Task<List<SoldProductsReportViewModel>> GetSoldProductsReport(DateOnly fromDate, DateOnly toDate)
        {
            var sales = await _dbContext.Sale
                .Where(p => p.Dismissed == null || p.Dismissed == false)
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
